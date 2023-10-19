namespace Admin.NET.Core.Service;

/// <summary>
/// 开放接口访问输入参数
/// </summary>
public class OpenAccessInput : BasePageInput
{
    /// <summary>
    /// 身份标识
    /// </summary>
    public string AccessKey { get; set; }
}