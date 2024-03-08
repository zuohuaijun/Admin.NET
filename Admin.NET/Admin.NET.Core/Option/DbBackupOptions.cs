// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core;

/// <summary>
/// 数据库备份配置选项
/// </summary>
public sealed class DbBackupOptions : IConfigurableOptions
{
    /// <summary>
    /// 服务器地址
    /// </summary>
    public string Host { get; set; }

    /// <summary>
    /// 端口
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// 数据库名称
    /// </summary>
    public string Database { get; set; }

    /// <summary>
    /// 账号
    /// </summary>
    public string User { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; }
}