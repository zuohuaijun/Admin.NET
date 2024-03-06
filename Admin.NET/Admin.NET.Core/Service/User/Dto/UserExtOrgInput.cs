// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core.Service;

public class UserExtOrgInput : BaseIdInput
{
    /// <summary>
    /// 机构Id
    /// </summary>
    public long OrgId { get; set; }

    /// <summary>
    /// 职位Id
    /// </summary>
    public long PosId { get; set; }

    /// <summary>
    /// 工号
    /// </summary>
    public string JobNum { get; set; }

    /// <summary>
    /// 职级
    /// </summary>
    public string PosLevel { get; set; }

    /// <summary>
    /// 入职日期
    /// </summary>
    public DateTime? JoinDate { get; set; }
}