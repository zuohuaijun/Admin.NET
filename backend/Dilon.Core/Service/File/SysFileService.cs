using Furion;
using Furion.DatabaseAccessor;
using Furion.DatabaseAccessor.Extensions;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Furion.RemoteRequest.Extensions;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnceMi.AspNetCore.OSS;
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

        private readonly IOSSServiceFactory _oSSServiceFactory;

        private readonly string bucketName = "bucketName";

        public SysFileService(IRepository<SysFile> sysFileInfoRep,
            IConfiguration configuration,
            IOSSServiceFactory oSSServiceFactory)
        {
            _sysFileInfoRep = sysFileInfoRep;
            _configuration = configuration;
            _oSSServiceFactory = oSSServiceFactory;
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

                switch (file.FileLocation)
                {
                    case (int)FileLocation.MINIO:
                        await _oSSServiceFactory.Create().RemoveObjectAsync(bucketName, Path.Combine(file.FileBucket, file.FileObjectName));
                        break;

                    case (int)FileLocation.ALIYUN:
                        await _oSSServiceFactory.Create("Aliyun").RemoveObjectAsync(bucketName, Path.Combine(file.FileBucket, file.FileObjectName));
                        break;

                    case (int)FileLocation.TENCENT:
                        await _oSSServiceFactory.Create("QCloud").RemoveObjectAsync(bucketName, Path.Combine(file.FileBucket, file.FileObjectName));
                        break;

                    default:
                        var filePath = Path.Combine(App.WebHostEnvironment.WebRootPath, file.FilePath, file.FileObjectName);
                        if (File.Exists(filePath))
                            File.Delete(filePath);
                        break;
                }
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
            // 可以读取系统配置来决定将文件存储到什么地方
            await UploadFile(file, _configuration["UploadFile:Default:path"], FileLocation.LOCAL);
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
            var filePath = Path.Combine(App.WebHostEnvironment.WebRootPath, file.FilePath, file.FileObjectName);
            var fileName = HttpUtility.UrlEncode(file.FileOriginName, Encoding.GetEncoding("UTF-8"));

            switch (file.FileLocation)
            {
                case (int)FileLocation.ALIYUN:
                    var stream1 = await (await _oSSServiceFactory.Create("Aliyun").PresignedGetObjectAsync(bucketName, filePath, 5)).GetAsStreamAsync();
                    return new FileStreamResult(stream1, "application/octet-stream") { FileDownloadName = fileName };

                case (int)FileLocation.TENCENT:
                    var stream2 = await (await _oSSServiceFactory.Create("QCloud").PresignedGetObjectAsync(bucketName, filePath, 5)).GetAsStreamAsync();
                    return new FileStreamResult(stream2, "application/octet-stream") { FileDownloadName = fileName };

                case (int)FileLocation.MINIO:
                    var stream3 = await (await _oSSServiceFactory.Create().PresignedGetObjectAsync(file.FileBucket, filePath, 5)).GetAsStreamAsync();
                    return new FileStreamResult(stream3, "application/octet-stream") { FileDownloadName = fileName };

                default:
                    var path = Path.Combine(App.WebHostEnvironment.WebRootPath, filePath);
                    return new FileStreamResult(new FileStream(path, FileMode.Open), "application/octet-stream") { FileDownloadName = fileName };
            }
        }

        /// <summary>
        /// 上传头像
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("/sysFileInfo/uploadAvatar")]
        public async Task<long> UploadFileAvatar(IFormFile file)
        {
            return await UploadFile(file, _configuration["UploadFile:Avatar:path"]);
        }

        /// <summary>
        /// 上传文档
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("/sysFileInfo/uploadDocument")]
        public async Task UploadFileDocument(IFormFile file)
        {
            await UploadFile(file, _configuration["UploadFile:Document:path"]);
        }

        /// <summary>
        /// 上传商店图片
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("/sysFileInfo/uploadShop")]
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
        private static async Task<long> UploadFile(IFormFile file, string pathType)
        {
            var filePath = Path.Combine(App.WebHostEnvironment.WebRootPath, pathType);
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            var fileSizeKb = (long)(file.Length / 1024.0); // 文件大小KB
            var originalFilename = file.FileName; // 文件原始名称
            var fileSuffix = Path.GetExtension(file.FileName).ToLower(); // 文件后缀

            // 先存库获取Id
            var newFile = await new SysFile
            {
                FileLocation = (int)FileLocation.LOCAL,
                FileBucket = FileLocation.LOCAL.ToString(),
                //FileObjectName = finalName,
                FileOriginName = originalFilename,
                FileSuffix = fileSuffix.TrimStart('.'),
                FileSizeKb = fileSizeKb.ToString(),
                FilePath = pathType
            }.InsertNowAsync();

            var finalName = newFile.Entity.Id + fileSuffix; // 生成文件的最终名称
            using (var stream = File.Create(Path.Combine(filePath, finalName)))
            {
                await file.CopyToAsync(stream);
            }

            newFile.Entity.FileObjectName = finalName;
            return newFile.Entity.Id; // 返回文件唯一标识
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file">文件</param>
        /// <param name="pathType">存储路径</param>
        /// <param name="fileLocation">文件存储位置</param>
        /// <returns></returns>
        private async Task<long> UploadFile(IFormFile file, string pathType, FileLocation fileLocation)
        {
            var fileSizeKb = (long)(file.Length / 1024.0); // 文件大小KB
            var originalFilename = file.FileName; // 文件原始名称
            var fileSuffix = Path.GetExtension(file.FileName).ToLower(); // 文件后缀

            // 先存库获取Id
            var newFile = await new SysFile
            {
                FileLocation = (int)FileLocation.LOCAL,
                FileBucket = FileLocation.LOCAL.ToString(),
                //FileObjectName = finalName,
                FileOriginName = originalFilename,
                FileSuffix = fileSuffix.TrimStart('.'),
                FileSizeKb = fileSizeKb.ToString(),
                FilePath = pathType
            }.InsertNowAsync();

            var finalName = newFile.Entity.Id + fileSuffix; // 生成文件的最终名称
            switch (fileLocation)
            {
                case FileLocation.ALIYUN:
                    var filePath = Path.Combine(pathType, finalName);
                    var stream = file.OpenReadStream();
                    await _oSSServiceFactory.Create("aliyun").PutObjectAsync(bucketName, filePath, stream);
                    break;

                case FileLocation.TENCENT:
                    var filePath1 = Path.Combine(pathType, finalName);
                    var stream1 = file.OpenReadStream();
                    await _oSSServiceFactory.Create("qcloud").PutObjectAsync(bucketName, filePath1, stream1);
                    break;

                case FileLocation.MINIO:
                    var filePath2 = Path.Combine(pathType, finalName);
                    var stream2 = file.OpenReadStream();
                    await _oSSServiceFactory.Create().PutObjectAsync(bucketName, filePath2, stream2);
                    break;

                default:
                    var filePath4 = Path.Combine(App.WebHostEnvironment.WebRootPath, pathType);
                    if (!Directory.Exists(filePath4))
                        Directory.CreateDirectory(filePath4);
                    using (var stream4 = File.Create(Path.Combine(filePath4, finalName)))
                    {
                        await file.CopyToAsync(stream4);
                    }
                    break;
            }
            newFile.Entity.FileObjectName = finalName;
            return newFile.Entity.Id; // 返回文件唯一标识
        }
    }
}