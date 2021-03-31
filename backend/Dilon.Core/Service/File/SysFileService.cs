using Dilon.Core.Entity.System;
using Furion;
using Furion.DatabaseAccessor;
using Furion.DatabaseAccessor.Extensions;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Furion.Snowflake;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Dilon.Core.Service
{
    /// <summary>
    /// 文件服务
    /// </summary>
    [ApiDescriptionSettings(Name = "File", Order = 100)]
    public class SysFileService : ISysFileService, IDynamicApiController, ITransient
    {
        private readonly IRepository<SysFile> _sysFileInfoRep;  // 文件信息表仓储 

        private readonly IConfiguration _configuration;

        public SysFileService(IRepository<SysFile> sysFileInfoRep, IConfiguration configuration)
        {
            _sysFileInfoRep = sysFileInfoRep;
            _configuration = configuration;
        }

        /// <summary>
        /// 分页获取文件列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysFileInfo/page")]
        public async Task<dynamic> QueryFileInfoPageList([FromQuery] FileInput input)
        {
            var fileBucket = !string.IsNullOrEmpty(input.FileBucket?.Trim());
            var fileOriginName = !string.IsNullOrEmpty(input.FileOriginName?.Trim());
            var files = await _sysFileInfoRep.DetachedEntities
                                             .Where(input.FileLocation > 0, u => u.FileLocation == input.FileLocation)
                                             .Where(fileBucket, u => EF.Functions.Like(u.FileBucket, $"%{input.FileBucket.Trim()}%"))
                                             .Where(fileOriginName, u => EF.Functions.Like(u.FileOriginName, $"%{input.FileOriginName.Trim()}%"))
                                             .Select(u => u.Adapt<FileOutput>())
                                             .ToPagedListAsync(input.PageNo, input.PageSize);
            return XnPageResult<FileOutput>.PageResult(files);
        }

        /// <summary>
        /// 获取文件列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysFileInfo/list")]
        public async Task<List<SysFile>> GetFileInfoList([FromQuery] FileOutput input)
        {
            return await _sysFileInfoRep.DetachedEntities.ToListAsync();
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysFileInfo/delete")]
        public async Task DeleteFileInfo(DeleteFileInfoInput input)
        {
            var file = await _sysFileInfoRep.FirstOrDefaultAsync(u => u.Id == input.Id);
            if (file != null)
            {
                await file.DeleteAsync();

                var filePath = Path.Combine(App.WebHostEnvironment.WebRootPath, file.FileBucket, file.FileObjectName);
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
        }

        /// <summary>
        /// 获取文件详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysFileInfo/detail")]
        public async Task<SysFile> GetFileInfo([FromQuery] QueryFileInoInput input)
        {
            var file = await _sysFileInfoRep.FirstOrDefaultAsync(u => u.Id == input.Id);
            if (file == null)
                throw Oops.Oh(ErrorCode.D8000);
            return file;
        }

        /// <summary>
        /// 预览文件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysFileInfo/preview")]
        public async Task<IActionResult> PreviewFileInfo([FromQuery] QueryFileInoInput input)
        {
            return await DownloadFileInfo(input);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("/sysFileInfo/upload")]
        public async Task UploadFileDefault(IFormFile file)
        {
            await UploadFile(file, _configuration["UploadFile:Default:path"]);
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysFileInfo/download")]
        public async Task<IActionResult> DownloadFileInfo([FromQuery] QueryFileInoInput input)
        {
            var file = await GetFileInfo(input);
            var filePath = Path.Combine(App.WebHostEnvironment.WebRootPath, file.FileBucket, file.FileObjectName);
            var fileName = HttpUtility.UrlEncode(file.FileOriginName, Encoding.GetEncoding("UTF-8"));
            return new FileStreamResult(new FileStream(filePath, FileMode.Open), "application/octet-stream") { FileDownloadName = fileName };
        }

        /// <summary>
        /// 上传头像
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task UploadFileAvatar(IFormFile file)
        {
            await UploadFile(file, _configuration["UploadFile:Avatar:path"]);
        }

        /// <summary>
        /// 上传文档
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task UploadFileDocument(IFormFile file)
        {
            await UploadFile(file, _configuration["UploadFile:Document:path"]);
        }

        /// <summary>
        /// 上传商店图片
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task UploadFileShop(IFormFile file)
        {
            await UploadFile(file, _configuration["UploadFile:Shop:path"]);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="pathType"></param>
        /// <returns></returns>
        private static async Task UploadFile(IFormFile file, string pathType)
        {
            var fileId = IDGenerator.NextId();

            var fileSizeKb = (long)(file.Length / 1024.0); // 文件大小KB
            var originalFilename = file.FileName; // 文件原始名称
            var fileSuffix = Path.GetExtension(file.FileName).ToLower(); // 文件后缀
            var finalName = fileId + fileSuffix; // 生成文件的最终名称            

            var filePath = Path.Combine(App.WebHostEnvironment.WebRootPath, pathType);
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            using (var stream = File.Create(Path.Combine(filePath, finalName)))
            {
                await file.CopyToAsync(stream);
            }

            var sysFileInfo = new SysFile
            {
                Id = fileId,
                FileLocation = (int)FileLocation.LOCAL,
                FileBucket = pathType,
                FileObjectName = finalName,
                FileOriginName = originalFilename,
                FileSuffix = fileSuffix.TrimStart('.'),
                FileSizeKb = fileSizeKb
            };
            await sysFileInfo.InsertAsync();
        }
    }
}
