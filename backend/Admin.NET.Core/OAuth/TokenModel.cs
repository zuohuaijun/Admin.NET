using System.Text.Json.Serialization;

namespace Admin.NET.Core.OAuth
{
    /// <summary>
    /// AccessToken参数
    /// </summary>
    public class TokenModel
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        [JsonPropertyName("openid")]
        public string OpenId { get; set; }

        /// <summary>
        /// Token 类型
        /// </summary>
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        /// <summary>
        /// AccessToken
        /// </summary>
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// 用于刷新 AccessToken 的 Token
        /// </summary>
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }

        /// <summary>
        /// 此 AccessToken 对应的权限
        /// </summary>
        [JsonPropertyName("scope")]
        public string Scope { get; set; }

        /// <summary>
        /// AccessToken 过期时间
        /// </summary>
        [JsonPropertyName("expires_in")]
        public dynamic ExpiresIn { get; set; }

        /// <summary>
        /// 错误的详细描述
        /// </summary>
        [JsonPropertyName("error_description")]
        public string ErrorDescription { get; set; }
    }

    public static class AccessTokenModelModelExtensions
    {
        /// <summary>
        /// 获取的Token是否包含错误
        /// </summary>
        /// <param name="accessTokenModel"></param>
        /// <returns></returns>
        public static bool HasError(this TokenModel accessTokenModel)
        {
            return accessTokenModel == null ||
                   string.IsNullOrEmpty(accessTokenModel.AccessToken) ||
                   !string.IsNullOrEmpty(accessTokenModel.ErrorDescription);
        }
    }
}