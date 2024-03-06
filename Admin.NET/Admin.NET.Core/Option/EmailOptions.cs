// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core;

/// <summary>
/// 邮件配置选项
/// </summary>
public sealed class EmailOptions : IConfigurableOptions
{
    /// <summary>
    /// 主机
    /// </summary>
    public string Host { get; set; }

    /// <summary>
    /// 端口
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// 默认发件者邮箱
    /// </summary>
    public string DefaultFromEmail { get; set; }

    /// <summary>
    /// 默认接收人邮箱
    /// </summary>
    public string DefaultToEmail { get; set; }

    /// <summary>
    /// 启用SSL
    /// </summary>
    public bool EnableSsl { get; set; }

    ///// <summary>
    ///// 是否使用默认凭据
    ///// </summary>
    //public bool UseDefaultCredentials { get; set; }

    /// <summary>
    /// 邮箱账号
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 邮箱密码
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// 默认邮件标题
    /// </summary>
    public string DefaultFromName { get; set; }
}