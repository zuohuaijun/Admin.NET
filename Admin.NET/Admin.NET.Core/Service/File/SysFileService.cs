// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Aliyun.OSS.Util;
using Furion.VirtualFileServer;
using OnceMi.AspNetCore.OSS;

namespace Admin.NET.Core.Service;

/// <summary>
/// 系统文件服务
/// </summary>
[ApiDescriptionSettings(Order = 410)]
public class SysFileService : IDynamicApiController, ITransient
{
    private readonly UserManager _userManager;
    private readonly SqlSugarRepository<SysFile> _sysFileRep;
    private readonly OSSProviderOptions _OSSProviderOptions;
    private readonly UploadOptions _uploadOptions;
    private readonly IOSSService _OSSService;

    public SysFileService(UserManager userManager,
        SqlSugarRepository<SysFile> sysFileRep,
        IOptions<OSSProviderOptions> oSSProviderOptions,
        IOptions<UploadOptions> uploadOptions,
        IOSSServiceFactory ossServiceFactory)
    {
        _userManager = userManager;
        _sysFileRep = sysFileRep;
        _OSSProviderOptions = oSSProviderOptions.Value;
        _uploadOptions = uploadOptions.Value;
        if (_OSSProviderOptions.IsEnable)
            _OSSService = ossServiceFactory.Create(Enum.GetName(_OSSProviderOptions.Provider));
    }

    /// <summary>
    /// 获取文件分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取文件分页列表")]
    public async Task<SqlSugarPagedList<SysFile>> Page(PageFileInput input)
    {
        return await _sysFileRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.FileName), u => u.FileName.Contains(input.FileName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.StartTime.ToString()) && !string.IsNullOrWhiteSpace(input.EndTime.ToString()),
                        u => u.CreateTime >= input.StartTime && u.CreateTime <= input.EndTime)
            .OrderBy(u => u.CreateTime, OrderByType.Desc)
            .ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="file"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    [DisplayName("上传文件")]
    public async Task<FileOutput> UploadFile([Required] IFormFile file, [FromQuery] string? path)
    {
        var sysFile = await HandleUploadFile(file, path);
        return new FileOutput
        {
            Id = sysFile.Id,
            Url = sysFile.Url, // string.IsNullOrWhiteSpace(sysFile.Url) ? _commonService.GetFileUrl(sysFile) : sysFile.Url,
            SizeKb = sysFile.SizeKb,
            Suffix = sysFile.Suffix,
            FilePath = sysFile.FilePath,
            FileName = sysFile.FileName
        };
    }

    /// <summary>
    /// 上传文件Base64
    /// </summary>
    /// <param name="strBase64"></param>
    /// <param name="fileName"></param>
    /// <param name="contentType"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    private async Task<FileOutput> UploadFileFromBase64(string strBase64, string fileName, string contentType, string? path)
    {
        byte[] fileData = Convert.FromBase64String(strBase64);
        var ms = new MemoryStream();
        ms.Write(fileData);
        ms.Seek(0, SeekOrigin.Begin);
        if (string.IsNullOrEmpty(fileName))
            fileName = $"{YitIdHelper.NextId()}.jpg";
        if (string.IsNullOrEmpty(contentType))
            contentType = "image/jpg";
        IFormFile formFile = new FormFile(ms, 0, fileData.Length, "file", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = contentType
        };
        return await UploadFile(formFile, path);
    }

    /// <summary>
    /// 上传文件Base64
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("上传文件Base64")]
    [HttpPost]
    public async Task<FileOutput> UploadFileFromBase64(UploadFileFromBase64Input input)
    {
        return await UploadFileFromBase64(input.FileDataBase64, input.FileName, input.ContentType, input.Path);
    }

    /// <summary>
    /// 上传多文件
    /// </summary>
    /// <param name="files"></param>
    /// <returns></returns>
    [DisplayName("上传多文件")]
    public async Task<List<FileOutput>> UploadFiles([Required] List<IFormFile> files)
    {
        var filelist = new List<FileOutput>();
        foreach (var file in files)
        {
            filelist.Add(await UploadFile(file, ""));
        }
        return filelist;
    }

    /// <summary>
    /// 根据文件Id或Url下载
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("根据文件Id或Url下载")]
    public async Task<IActionResult> DownloadFile(FileInput input)
    {
        var file = input.Id > 0 ? await GetFile(input) : await _sysFileRep.GetFirstAsync(u => u.Url == input.Url);
        var fileName = HttpUtility.UrlEncode(file.FileName, Encoding.GetEncoding("UTF-8"));
        if (_OSSProviderOptions.IsEnable)
        {
            var filePath = string.Concat(file.FilePath, "/", file.Id.ToString() + file.Suffix);
            var stream = await (await _OSSService.PresignedGetObjectAsync(file.BucketName.ToString(), filePath, 5)).GetAsStreamAsync();
            return new FileStreamResult(stream.Stream, "application/octet-stream") { FileDownloadName = fileName + file.Suffix };
        }
        else
        {
            var filePath = Path.Combine(file.FilePath, file.Id.ToString() + file.Suffix);
            var path = Path.Combine(App.WebHostEnvironment.WebRootPath, filePath);
            return new FileStreamResult(new FileStream(path, FileMode.Open), "application/octet-stream") { FileDownloadName = fileName + file.Suffix };
        }
    }

    /// <summary>
    /// 删除文件
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    [DisplayName("删除文件")]
    public async Task DeleteFile(DeleteFileInput input)
    {
        var file = await _sysFileRep.GetFirstAsync(u => u.Id == input.Id);
        if (file != null)
        {
            await _sysFileRep.DeleteAsync(file);

            if (_OSSProviderOptions.IsEnable)
            {
                await _OSSService.RemoveObjectAsync(file.BucketName.ToString(), string.Concat(file.FilePath, "/", $"{input.Id}{file.Suffix}"));
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
    /// 更新文件
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    [DisplayName("更新文件")]
    public async Task UpdateFile(FileInput input)
    {
        var isExist = await _sysFileRep.IsAnyAsync(u => u.Id == input.Id);
        if (!isExist) throw Oops.Oh(ErrorCodeEnum.D8000);

        await _sysFileRep.UpdateAsync(u => new SysFile() { FileName = input.FileName }, u => u.Id == input.Id);
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
    /// <param name="savePath">路径</param>
    /// <returns></returns>
    private async Task<SysFile> HandleUploadFile(IFormFile file, string savePath)
    {
        if (file == null) throw Oops.Oh(ErrorCodeEnum.D8000);

        // 判断是否重复上传的文件
        var sizeKb = (long)(file.Length / 1024.0); // 大小KB
        var fileMd5 = string.Empty;
        if (_uploadOptions.EnableMd5)
        {
            using var fileStream = file.OpenReadStream();
            fileMd5 = OssUtils.ComputeContentMd5(fileStream, fileStream.Length);
            var sysFile = await _sysFileRep.GetFirstAsync(u => u.FileMd5 == fileMd5 && (u.SizeKb == null || u.SizeKb == sizeKb.ToString()));
            if (sysFile != null) return sysFile;
        }

        var path = savePath;
        if (string.IsNullOrWhiteSpace(savePath))
        {
            path = _uploadOptions.Path;
            var reg = new Regex(@"(\{.+?})");
            var match = reg.Matches(path);
            match.ToList().ForEach(a =>
            {
                var str = DateTime.Now.ToString(a.ToString().Substring(1, a.Length - 2)); // 每天一个目录
                path = path.Replace(a.ToString(), str);
            });
        }

        if (!_uploadOptions.ContentType.Contains(file.ContentType))
            throw Oops.Oh(ErrorCodeEnum.D8001);

        if (sizeKb > _uploadOptions.MaxSize)
            throw Oops.Oh(ErrorCodeEnum.D8002);

        var suffix = Path.GetExtension(file.FileName).ToLower(); // 后缀
        if (string.IsNullOrWhiteSpace(suffix))
        {
            var contentTypeProvider = FS.GetFileExtensionContentTypeProvider();
            suffix = contentTypeProvider.Mappings.FirstOrDefault(u => u.Value == file.ContentType).Key;
            // 修改 image/jpeg 类型返回的 .jpe 后缀
            if (suffix == ".jpe")
                suffix = ".jpg";
        }
        if (string.IsNullOrWhiteSpace(suffix))
            throw Oops.Oh(ErrorCodeEnum.D8003);

        var newFile = new SysFile
        {
            Id = YitIdHelper.NextId(),
            // BucketName = _OSSProviderOptions.IsEnable ? _OSSProviderOptions.Provider.ToString() : "Local",
            // 阿里云对bucket名称有要求，1.只能包括小写字母，数字，短横线（-）2.必须以小写字母或者数字开头  3.长度必须在3-63字节之间
            // 无法使用Provider
            BucketName = _OSSProviderOptions.IsEnable ? _OSSProviderOptions.Bucket : "Local",
            FileName = Path.GetFileNameWithoutExtension(file.FileName),
            Suffix = suffix,
            SizeKb = sizeKb.ToString(),
            FilePath = path,
            FileMd5 = fileMd5,
        };

        var finalName = newFile.Id + suffix; // 文件最终名称
        if (_OSSProviderOptions.IsEnable)
        {
            newFile.Provider = Enum.GetName(_OSSProviderOptions.Provider);
            var filePath = string.Concat(path, "/", finalName);
            await _OSSService.PutObjectAsync(newFile.BucketName, filePath, file.OpenReadStream());
            //  http://<你的bucket名字>.oss.aliyuncs.com/<你的object名字>
            //  生成外链地址 方便前端预览
            switch (_OSSProviderOptions.Provider)
            {
                case OSSProvider.Aliyun:
                    newFile.Url = $"{(_OSSProviderOptions.IsEnableHttps ? "https" : "http")}://{newFile.BucketName}.{_OSSProviderOptions.Endpoint}/{filePath}";
                    break;

                case OSSProvider.Minio:
                    // 获取Minio文件的下载或者预览地址
                    // newFile.Url = await GetMinioPreviewFileUrl(newFile.BucketName, filePath);// 这种方法生成的Url是有7天有效期的，不能这样使用
                    // 需要在MinIO中的Buckets开通对 Anonymous 的readonly权限
                    newFile.Url = $"{(_OSSProviderOptions.IsEnableHttps ? "https" : "http")}://{_OSSProviderOptions.Endpoint}/{newFile.BucketName}/{filePath}";
                    break;
            }
        }
        else
        {
            newFile.Provider = ""; // 本地存储 Provider 显示为空
            var filePath = Path.Combine(App.WebHostEnvironment.WebRootPath, path);
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            var realFile = Path.Combine(filePath, finalName);
            //IDetector detector;
            using (var stream = File.Create(realFile))
            {
                await file.CopyToAsync(stream);
                //detector = stream.DetectFiletype();
            }
            //var realExt = detector.Extension; // 真实扩展名
            //// 二次校验扩展名
            //if (!string.Equals(realExt, suffix.Replace(".", ""), StringComparison.OrdinalIgnoreCase))
            //{
            //    var delFilePath = Path.Combine(App.WebHostEnvironment.WebRootPath, realFile);
            //    if (File.Exists(delFilePath))
            //        File.Delete(delFilePath);
            //    throw Oops.Oh(ErrorCodeEnum.D8001);
            //}

            // 生成外链
            newFile.Url = $"{CommonUtil.GetLocalhost()}/{newFile.FilePath}/{newFile.Id + newFile.Suffix}";
        }
        await _sysFileRep.AsInsertable(newFile).ExecuteCommandAsync();
        return newFile;
    }

    ///// <summary>
    ///// 获取Minio文件的下载或者预览地址
    ///// </summary>
    ///// <param name="bucketName">桶名</param>
    ///// <param name="fileName">文件名</param>
    ///// <returns></returns>
    //private async Task<string> GetMinioPreviewFileUrl(string bucketName, string fileName)
    //{
    //    return await _OSSService.PresignedGetObjectAsync(bucketName, fileName, 7);
    //}

    /// <summary>
    /// 上传头像
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    [DisplayName("上传头像")]
    public async Task<FileOutput> UploadAvatar([Required] IFormFile file)
    {
        var sysUserRep = _sysFileRep.ChangeRepository<SqlSugarRepository<SysUser>>();
        var user = sysUserRep.GetFirst(u => u.Id == _userManager.UserId);
        // 删除当前用户已有头像
        if (!string.IsNullOrWhiteSpace(user.Avatar))
        {
            var fileId = Path.GetFileNameWithoutExtension(user.Avatar);
            await DeleteFile(new DeleteFileInput { Id = long.Parse(fileId) });
        }

        var res = await UploadFile(file, "Upload/Avatar");
        await sysUserRep.UpdateAsync(u => new SysUser() { Avatar = $"{res.FilePath}/{res.Name}" }, u => u.Id == user.Id);
        return res;
    }

    /// <summary>
    /// 上传电子签名
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    [DisplayName("上传电子签名")]
    public async Task<FileOutput> UploadSignature([Required] IFormFile file)
    {
        var sysUserRep = _sysFileRep.ChangeRepository<SqlSugarRepository<SysUser>>();
        var user = sysUserRep.GetFirst(u => u.Id == _userManager.UserId);
        // 删除当前用户已有电子签名
        if (!string.IsNullOrWhiteSpace(user.Signature) && user.Signature.EndsWith(".png"))
        {
            var fileId = Path.GetFileNameWithoutExtension(user.Signature);
            await DeleteFile(new DeleteFileInput { Id = long.Parse(fileId) });
        }

        var res = await UploadFile(file, "Upload/Signature");
        await sysUserRep.UpdateAsync(u => new SysUser() { Signature = $"{res.FilePath}/{res.Name}" }, u => u.Id == user.Id);
        return res;
    }
}