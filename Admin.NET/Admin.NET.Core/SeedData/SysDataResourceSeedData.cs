namespace Admin.NET.Core;

/// <summary>
/// 系统数据资源种子数据
/// </summary>
public class SysDataResourceSeedData : ISqlSugarEntitySeedData<SysDataResource>
{
    /// <summary>
    /// 种子数据
    /// </summary>
    /// <returns></returns>
    public IEnumerable<SysDataResource> HasData()
    {
        return new[]
        {
            new SysDataResource{ Id=243848612100001, Pid=0, Name="行政区",Value="district", Code="1001", CreateTime=DateTime.Parse("2022-05-30 00:00:00"), Remark="行政区"},
            new SysDataResource{ Id=243848612100002, Pid=243848612100001, Name="北京市",Value="110000", Code="1001001", CreateTime=DateTime.Parse("2022-05-30 00:00:00"), Remark="北京市"},
            new SysDataResource{ Id=243848612100003, Pid=243848612100002, Name="东城区",Value="110101", Code="1001001001", CreateTime=DateTime.Parse("2022-05-30 00:00:00"), Remark="东城区"},
            new SysDataResource{ Id=243848612100004, Pid=0, Name="行业",Value="industry", Code="1002", CreateTime=DateTime.Parse("2022-05-30 00:00:00"), Remark="行业"},
            new SysDataResource{ Id=243848612100005, Pid=243848612100004, Name="一产",Value="一产", Code="1002001", CreateTime=DateTime.Parse("2022-05-30 00:00:00"), Remark="一产"},
            new SysDataResource{ Id=243848612100006, Pid=243848612100005, Name="农、林、牧、渔",Value="农、林、牧、渔", Code="1002001001", CreateTime=DateTime.Parse("2022-05-30 00:00:00"), Remark="农、林、牧、渔"},
        };
    }
}