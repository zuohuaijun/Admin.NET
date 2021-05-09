using System.Threading.Tasks;

namespace Admin.NET.Core.OAuth
{
    public interface IWechatOAuth
    {
        Task<TokenModel> GetAccessTokenAsync(string code, string state = "");

        string GetAuthorizeUrl(string state = "");

        Task<UserInfoModel> GetUserInfoAsync(string accessToken, string openId);

        Task<TokenModel> GetRefreshTokenAsync(string refreshToken);
    }
}