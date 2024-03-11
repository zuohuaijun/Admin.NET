// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core.Service;

/// <summary>
/// 授权角色机构
/// </summary>
public class RoleOrgInput : BaseIdInput
{
    /// <summary>
    /// 数据范围
    /// </summary>
    public int DataScope { get; set; }

    /// <summary>
    /// 机构Id集合
    /// </summary>
    public List<long> OrgIdList { get; set; }
}