// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core.Service;

public class TenantOutput : SysTenant
{
    /// <summary>
    /// 租户名称
    /// </summary>
    public virtual string Name { get; set; }

    /// <summary>
    /// 管理员账号
    /// </summary>
    public virtual string AdminAccount { get; set; }

    /// <summary>
    /// 电子邮箱
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// 电话
    /// </summary>
    public string Phone { get; set; }
}