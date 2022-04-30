using System;
using System.Collections.Generic;

namespace Admin.NET.Core
{
    /// <summary>
    /// 系统职位表种子数据
    /// </summary>
    public class SysPosSeedData : ISqlSugarEntitySeedData<SysPos>
    {
        /// <summary>
        /// 种子数据
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SysPos> HasData()
        {
            return new[]
            {
                new SysPos{ Id=252885263003720, Name="总经理", Code="zjl", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="总经理", TenantId=142307070918780 },
                new SysPos{ Id=252885263003721, Name="副总经理", Code="fzjl", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="副总经理", Status=StatusEnum.Disable, TenantId=142307070918780 },
                new SysPos{ Id=252885263003722, Name="部门经理", Code="bmjl", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="部门经理", TenantId=142307070918780 },
                new SysPos{ Id=252885263003723, Name="工作人员", Code="gzry", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="工作人员", TenantId=142307070918780 },
                new SysPos{ Id=252885263003724, Name="其他", Code="qt", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="其他", TenantId=142307070918780 },

                new SysPos{ Id=252885263003725, Name="总经理", Code="zjl", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="总经理", TenantId=142307070918781 },
                new SysPos{ Id=252885263003726, Name="副总经理", Code="fzjl", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="副总经理", Status=StatusEnum.Disable, TenantId=142307070918781 },
                new SysPos{ Id=252885263003727, Name="部门经理", Code="bmjl", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="部门经理", TenantId=142307070918781 },
                new SysPos{ Id=252885263003728, Name="工作人员", Code="gzry", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="工作人员", TenantId=142307070918781 },
                new SysPos{ Id=252885263003729, Name="其他", Code="qt", CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Remark="其他", TenantId=142307070918781 }
            };
        }
    }
}