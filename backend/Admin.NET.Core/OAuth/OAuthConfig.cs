using Microsoft.Extensions.Configuration;

namespace Admin.NET.Core.OAuth
{
    /// <summary>
    /// OAuth配置---此结构方便拓展
    /// </summary>
    public class OAuthConfig
    {
        /// <summary>
        /// AppId
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// Secret Key
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        /// 回调地址
        /// </summary>
        public string RedirectUri { get; set; }

        /// <summary>
        /// 权限范围
        /// </summary>
        public string Scope { get; set; }

        public static OAuthConfig LoadFrom(IConfiguration configuration, string prefix)
        {
            return With(appId: configuration[prefix + ":app_id"],
                        appKey: configuration[prefix + ":app_key"],
                        redirectUri: configuration[prefix + ":redirect_uri"],
                        scope: configuration[prefix + ":scope"]);
        }

        private static OAuthConfig With(string appId, string appKey, string redirectUri, string scope)
        {
            return new OAuthConfig()
            {
                AppId = appId,
                AppKey = appKey,
                RedirectUri = redirectUri,
                Scope = scope
            };
        }
    }
}