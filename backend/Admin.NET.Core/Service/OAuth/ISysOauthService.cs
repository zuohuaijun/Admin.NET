using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service.OAuth
{
    public interface ISysOauthService
    {
        Task<dynamic> GetWechatUserInfo([FromQuery] string token, [FromQuery] string openId);

        Task WechatLogin();

        Task WechatLoginCallback([FromQuery] string code, [FromQuery] string state, [FromQuery] string error_description = "");
    }
}