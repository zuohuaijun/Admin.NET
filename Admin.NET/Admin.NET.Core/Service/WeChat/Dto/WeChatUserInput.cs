namespace Admin.NET.Core.Service;

public class WeChatUserInput : BasePageInput
{
    /// <summary>
    /// 昵称
    /// </summary>
    public string NickName { get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    public string Mobile { get; set; }
}

public class DeleteWeChatUserInput : BaseIdInput
{
}