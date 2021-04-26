using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dilon.Core.Service
{
    public interface ISysFileService
    {
        Task DeleteFileInfo(DeleteFileInfoInput input);

        Task<IActionResult> DownloadFileInfo([FromQuery] QueryFileInoInput input);

        Task<SysFile> GetFileInfo([FromQuery] QueryFileInoInput input);

        Task<List<SysFile>> GetFileInfoList([FromQuery] FileOutput input);

        Task<IActionResult> PreviewFileInfo([FromQuery] QueryFileInoInput input);

        Task<dynamic> QueryFileInfoPageList([FromQuery] FileInput input);

        Task<long> UploadFileAvatar(IFormFile file);

        Task UploadFileDefault(IFormFile file);

        Task UploadFileDocument(IFormFile file);

        Task UploadFileShop(IFormFile file);
    }
}