using Furion;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Furion.RemoteRequest.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OnceMi.AspNetCore.OSS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Admin.NET.Core.Service
{
    /// <summary>
    /// 文件服务
    /// </summary>
    [ApiDescriptionSettings(Name = "文件服务", Order = 194)]
    public class SysFileService : IDynamicApiController, ITransient
    {
        private readonly SqlSugarRepository<SysFile> _sysFileRep;
        private readonly OSSProviderOptions _OSSProviderOptions;
        private readonly IOSSService _OSSService;
        private readonly UploadOptions _uploadOptions;
        private readonly ICommonService _commonService;

        public SysFileService(SqlSugarRepository<SysFile> sysFileRep,
            IOptions<OSSProviderOptions> oSSProviderOptions,
            IOSSServiceFactory ossServiceFactory,
            ICommonService commonService,
            IOptions<UploadOptions> uploadOptions)
        {
            _sysFileRep = sysFileRep;
            _OSSProviderOptions = oSSProviderOptions.Value;
            _commonService = commonService;
            if (_OSSProviderOptions.IsEnable)
                _OSSService = ossServiceFactory.Create(_OSSProviderOptions.Provider.ToString());
            _uploadOptions = uploadOptions.Value;
        }

        /// <summary>
        /// 获取文件分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysFile/pageList")]
        public async Task<SqlSugarPagedList<SysFile>> QueryFilePageList([FromQuery] PageFileInput input)
        {
            var files = await _sysFileRep.AsQueryable()
                .WhereIF(!string.IsNullOrWhiteSpace(input.FileName), u => u.FileName.Contains(input.FileName.Trim()))
                .WhereIF(!string.IsNullOrWhiteSpace(input.StartTime.ToString()) && !string.IsNullOrWhiteSpace(input.EndTime.ToString()),
                            u => u.CreateTime >= input.StartTime && u.CreateTime <= input.EndTime)
                .OrderBy(u => u.CreateTime, SqlSugar.OrderByType.Desc)
                .ToPagedListAsync(input.Page, input.PageSize);
            return files;
        }

        /// <summary>
        /// 获取文件列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("/sysFile/list")]
        public async Task<List<SysFile>> GetFileList()
        {
            return await _sysFileRep.GetListAsync();
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("/sysFile/upload")]
        [AllowAnonymous]
        public async Task<FileOutput> UploadFile([Required] IFormFile file)
        {
            var sysFile = await HandleUploadFile(file);
            return new FileOutput
            {
                Id = sysFile.Id,
                Url = _commonService.GetFileUrl(sysFile),
                SizeKb = sysFile.SizeKb,
                Suffix = sysFile.Suffix,
                FilePath = sysFile.FilePath,
            };
        }

        /// <summary>
        /// 下载文件(文件流)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysFile/download")]
        public async Task<IActionResult> DownloadFile(FileInput input)
        {
            var file = await GetFile(input);
            var fileName = HttpUtility.UrlEncode(file.FileName, Encoding.GetEncoding("UTF-8"));
            if (_OSSProviderOptions.IsEnable)
            {
                var filePath = string.Concat(file.FilePath, "/", input.Id.ToString() + file.Suffix);
                var stream = await (await _OSSService.PresignedGetObjectAsync(file.BucketName.ToString(), filePath, 5)).GetAsStreamAsync();
                return new FileStreamResult(stream, "application/octet-stream") { FileDownloadName = fileName };
            }
            else
            {
                var filePath = Path.Combine(file.FilePath, input.Id.ToString() + file.Suffix);
                var path = Path.Combine(App.WebHostEnvironment.WebRootPath, filePath);
                return new FileStreamResult(new FileStream(path, FileMode.Open), "application/octet-stream") { FileDownloadName = fileName };
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysFile/delete")]
        public async Task DeleteFile(DeleteFileInput input)
        {
            var file = await _sysFileRep.GetFirstAsync(u => u.Id == input.Id);
            if (file != null)
            {
                await _sysFileRep.DeleteAsync(file);

                if (_OSSProviderOptions.IsEnable)
                {
                    await _OSSService.RemoveObjectAsync(file.BucketName.ToString(), string.Concat(file.FilePath, "/", input.Id.ToString()));
                }
                else
                {
                    var filePath = Path.Combine(App.WebHostEnvironment.WebRootPath, file.FilePath, input.Id.ToString() + file.Suffix);
                    if (File.Exists(filePath))
                        File.Delete(filePath);
                }
            }
        }

        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task<SysFile> GetFile([FromQuery] FileInput input)
        {
            var file = await _sysFileRep.GetFirstAsync(u => u.Id == input.Id);
            return file ?? throw Oops.Oh(ErrorCodeEnum.D8000);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file">文件</param>
        /// <returns></returns>
        private async Task<SysFile> HandleUploadFile(IFormFile file)
        {
            if (file == null) throw Oops.Oh(ErrorCodeEnum.D8000);

            string path = _uploadOptions.Path;
            Regex reg = new Regex(@"(\{.+?})");
            var match = reg.Matches(path);
            match.ToList().ForEach(a =>
            {
                var str = DateTime.Now.ToString(a.ToString().Substring(1, a.Length - 2));
                path = path.Replace(a.ToString(), str);
            });

            if (!_uploadOptions.ContentType.Contains(file.ContentType))
                throw Oops.Oh(ErrorCodeEnum.D8001);

            var sizeKb = (long)(file.Length / 1024.0); // 大小KB
            if (sizeKb > _uploadOptions.MaxSize) throw Oops.Oh(ErrorCodeEnum.D8002);

            var suffix = Path.GetExtension(file.FileName).ToLower(); // 后缀

            // 先存库获取Id
            var newFile = await _sysFileRep.AsInsertable(new SysFile
            {
                BucketName = _OSSProviderOptions.IsEnable ? _OSSProviderOptions.Provider.ToString() : "Local",
                FileName = Path.GetFileNameWithoutExtension(file.FileName),
                Suffix = suffix,
                SizeKb = sizeKb.ToString(),
                FilePath = path
            }).ExecuteReturnEntityAsync();

            var finalName = newFile.Id + suffix; // 文件最终名称
            if (_OSSProviderOptions.IsEnable)
            {
                var filePath = string.Concat(path, "/", finalName);
                await _OSSService.PutObjectAsync(newFile.BucketName.ToString(), filePath, file.OpenReadStream());
            }
            else
            {
                var filePath = Path.Combine(App.WebHostEnvironment.WebRootPath, path);
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);
                using var stream = File.Create(Path.Combine(filePath, finalName));
                await file.CopyToAsync(stream);
            }
            return newFile;
        }
    }
}