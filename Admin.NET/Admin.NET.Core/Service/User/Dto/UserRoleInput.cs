// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core.Service;

/// <summary>
/// 授权用户角色
/// </summary>
public class UserRoleInput
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// 角色Id集合
    /// </summary>
    public List<long> RoleIdList { get; set; }
}