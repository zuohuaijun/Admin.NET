namespace Admin.NET.Core.Service
{
    /// <summary>
    /// OAuth用户参数
    /// </summary>
    public class AuthUserInput
    {
        public string Uuid { get; set; }
        public string Username { get; set; }
        public string Nickname { get; set; }
        public string Avatar { get; set; }
        public string Blog { get; set; }
        public string Company { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }
        public string Eemark { get; set; }
        public Gender Gender { get; set; }
        public string Source { get; set; }
        public AuthToken Token { get; set; }
        public string RawUserInfo { get; set; }
    }
}