namespace Admin.NET.Core.Service;

/// <summary>
/// 授权用户角色
/// </summary>
public class UserRoleInput : BaseIdInput
{
    /// <summary>
    /// 机构Id
    /// </summary>
    public long OrgId { get; set; }

    /// <summary>
    /// 角色Id列表
    /// </summary>
    public List<long> RoleIdList { get; set; }
}