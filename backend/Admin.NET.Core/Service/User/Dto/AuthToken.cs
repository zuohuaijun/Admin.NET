namespace Admin.NET.Core.Service
{
    /// <summary>
    /// AuthToken参数
    /// </summary>
    public class AuthToken
    {
        public string AccessToken { get; set; }
        public int ExpireIn { get; set; }
        public string RefreshToken { get; set; }
        public string Uid { get; set; }
        public string OpenId { get; set; }
        public string AccessCode { get; set; }
        public string UnionId { get; set; }
        public string Scope { get; set; }
        public string TokenType { get; set; }
        public string IdToken { get; set; }
        public string MacAlgorithm { get; set; }
        public string MacKey { get; set; }
        public string Code { get; set; }
        public string OauthToken { get; set; }
        public string OauthTokenSecret { get; set; }
        public string UserId { get; set; }
        public string ScreenName { get; set; }
        public bool OauthCallbackConfirmed { get; set; }
    }
}