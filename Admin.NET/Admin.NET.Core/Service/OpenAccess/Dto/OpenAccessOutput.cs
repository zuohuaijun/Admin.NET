// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core.Service;

public class OpenAccessOutput : SysOpenAccess
{
    /// <summary>
    /// 绑定用户账号
    /// </summary>
    public string BindUserAccount { get; set; }

    /// <summary>
    /// 绑定租户名称
    /// </summary>
    public string BindTenantName { get; set; }
}