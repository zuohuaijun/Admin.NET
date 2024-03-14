// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

using System.Security.Claims;

namespace Admin.NET.Core.Service;

/// <summary>
/// 权限验证
/// </summary>
public class IdentityService : ITransient
{
    private readonly IHttpContextAccessor _context;
    private readonly List<APIJSON_Role> _roles;

    public IdentityService(IHttpContextAccessor context, IOptions<APIJSONOptions> roles)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _roles = roles.Value.Roles;
    }

    /// <summary>
    /// 获取当前用户Id
    /// </summary>
    /// <returns></returns>
    public string GetUserIdentity()
    {
        return _context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    /// <summary>
    /// 获取当前用户权限名称
    /// </summary>
    /// <returns></returns>
    public string GetUserRoleName()
    {
        return _context.HttpContext.User.FindFirstValue(ClaimTypes.Role);
    }

    /// <summary>
    /// 获取当前用户权限
    /// </summary>
    /// <returns></returns>
    public APIJSON_Role GetRole()
    {
        var role = string.IsNullOrEmpty(GetUserRoleName())
            ? _roles.FirstOrDefault()
            : _roles.FirstOrDefault(it => it.RoleName.Equals(GetUserRoleName(), StringComparison.CurrentCultureIgnoreCase));
        return role;
    }

    /// <summary>
    /// 获取当前表的可查询字段
    /// </summary>
    /// <param name="table"></param>
    /// <returns></returns>
    public (bool, string) GetSelectRole(string table)
    {
        var role = GetRole();
        if (role == null || role.Select == null || role.Select.Table == null)
            return (false, $"appsettings.json权限配置不正确！");

        var tablerole = role.Select.Table.FirstOrDefault(it => it == "*" || it.Equals(table, StringComparison.CurrentCultureIgnoreCase));
        if (string.IsNullOrEmpty(tablerole))
            return (false, $"表名{table}没权限查询！");

        var index = Array.IndexOf(role.Select.Table, tablerole);
        var selectrole = role.Select.Column[index];
        return (true, selectrole);
    }

    /// <summary>
    /// 当前列是否在角色里面
    /// </summary>
    /// <param name="col"></param>
    /// <param name="selectrole"></param>
    /// <returns></returns>
    public bool ColIsRole(string col, string[] selectrole)
    {
        if (selectrole.Contains("*")) return true;

        if (col.Contains('(') && col.Contains(')'))
        {
            var reg = new Regex(@"\(([^)]*)\)");
            var match = reg.Match(col);
            return selectrole.Contains(match.Result("$1"), StringComparer.CurrentCultureIgnoreCase);
        }
        else
        {
            return selectrole.Contains(col, StringComparer.CurrentCultureIgnoreCase);
        }
    }
}