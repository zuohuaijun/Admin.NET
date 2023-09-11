// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

namespace Admin.NET.Core;

/// <summary>
/// 系统机构表种子数据
/// </summary>
public class SysOrgSeedData : ISqlSugarEntitySeedData<SysOrg>
{
    /// <summary>
    /// 种子数据
    /// </summary>
    /// <returns></returns>
    public IEnumerable<SysOrg> HasData()
    {
        return new[]
        {
            new SysOrg{ Id=1300000000101, Pid=0, Name="大名科技", Code="1001", Type="101", Level=1, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="大名科技", TenantId=1300000000001 },
            new SysOrg{ Id=1300000000102, Pid=1300000000101, Name="市场部", Code="100101", Level=2, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="市场部", TenantId=1300000000001 },
            new SysOrg{ Id=1300000000103, Pid=1300000000101, Name="研发部", Code="100102", Level=2, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="研发部", TenantId=1300000000001 },
            new SysOrg{ Id=1300000000104, Pid=1300000000101, Name="财务部", Code="100103", Level=2, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="财务部", TenantId=1300000000001 },
            new SysOrg{ Id=1300000000105, Pid=1300000000104, Name="财务部1", Code="10010301", Level=3, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="财务部1", TenantId=1300000000001 },
            new SysOrg{ Id=1300000000106, Pid=1300000000104, Name="财务部2", Code="10010302", Level=3, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="财务部2", TenantId=1300000000001 },

            new SysOrg{ Id=1300000000201, Pid=0, Name="分公司1", Code="1002", Type="201", Level=1, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="分公司1", TenantId=1300000000001 },
            new SysOrg{ Id=1300000000202, Pid=1300000000201, Name="市场部", Code="100201", Level=2, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="市场部", TenantId=1300000000001 },
            new SysOrg{ Id=1300000000203, Pid=1300000000201, Name="研发部", Code="100202", Level=2, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="研发部", TenantId=1300000000001 },
            new SysOrg{ Id=1300000000204, Pid=1300000000201, Name="财务部", Code="100203", Level=2, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="财务部", TenantId=1300000000001 },

            new SysOrg{ Id=1300000000301, Pid=0, Name="分公司2", Code="1003", Type="201", Level=1, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="分公司2", TenantId=1300000000001 },
            new SysOrg{ Id=1300000000302, Pid=1300000000301, Name="市场部", Code="100301", Level=2, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="市场部", TenantId=1300000000001 },
            new SysOrg{ Id=1300000000303, Pid=1300000000301, Name="研发部", Code="100302", Level=2, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="市场部", TenantId=1300000000001 },
            new SysOrg{ Id=1300000000304, Pid=1300000000301, Name="财务部", Code="100303", Level=2, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="市场部", TenantId=1300000000001 },
        };
    }
}