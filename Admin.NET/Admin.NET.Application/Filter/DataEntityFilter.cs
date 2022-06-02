namespace Admin.NET.Application.Filter;

/// <summary>
/// 自定义业务实体过滤器(XXX数据)
/// </summary>
public class DataEntityFilter : IEntityFilter
{
    public IEnumerable<TableFilterItem<object>> AddEntityFilter()
    {
        // 当前用户所属机构
        var orgName = App.User?.FindFirst(ClaimConst.OrgName)?.Value;
        if (string.IsNullOrWhiteSpace(orgName))
            return null;

        // 构造自定义条件的过滤器
        Expression<Func<Test, bool>> dynamicExpression = u => u.Name.Contains(orgName);
        var tableFilterItem = new TableFilterItem<object>(typeof(Test), dynamicExpression);

        return new[]
        {
            tableFilterItem
        };
    }
}
