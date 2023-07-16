// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using System.Security.Claims;

namespace Admin.NET.Core.Service;

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
        var role = new APIJSON_Role();
        if (string.IsNullOrEmpty(GetUserRoleName())) // 若没登录默认取第一个
        {
            role = _roles.FirstOrDefault();
        }
        else
        {
            role = _roles.FirstOrDefault(it => it.RoleName.Equals(GetUserRoleName(), StringComparison.CurrentCultureIgnoreCase));
        }
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
        {
            return (false, $"appsettings.json权限配置不正确！");
        }
        string tablerole = role.Select.Table.FirstOrDefault(it => it == "*" || it.Equals(table, StringComparison.CurrentCultureIgnoreCase));

        if (string.IsNullOrEmpty(tablerole))
        {
            return (false, $"表名{table}没权限查询！");
        }
        int index = Array.IndexOf(role.Select.Table, tablerole);
        string selectrole = role.Select.Column[index];
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
        if (selectrole.Contains("*"))
        {
            return true;
        }
        else
        {
            if (col.Contains("(") && col.Contains(")"))
            {
                Regex reg = new Regex(@"\(([^)]*)\)");
                Match m = reg.Match(col);
                return selectrole.Contains(m.Result("$1"), StringComparer.CurrentCultureIgnoreCase);
            }
            else
            {
                return selectrole.Contains(col, StringComparer.CurrentCultureIgnoreCase);
            }
        }
    }
}