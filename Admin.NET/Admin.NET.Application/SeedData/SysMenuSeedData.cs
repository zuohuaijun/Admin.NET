namespace Admin.NET.Application.SeedData;

/// <summary>
/// 系统菜单表种子数据
/// </summary>
public class SysMenuSeedData : ISqlSugarEntitySeedData<SysMenu>
{
    /// <summary>
    /// 种子数据
    /// </summary>
    /// <returns></returns>
    public IEnumerable<SysMenu> HasData()
    {
        return new[]
        {
            new SysMenu{ Id=252885263002200, Pid=0, Title="业务测试", Path="/test", Name="test", Component="Layout", Redirect="/test/XXX", Icon="ele-Position", Type=MenuTypeEnum.Dir, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=20 },
            new SysMenu{ Id=252885263002201, Pid=252885263002200, Title="XXX业务", Path="/test/XXX", Name="XXX", Component="/test/XXX/index", Icon="ele-OfficeBuilding", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
        };
    }
}