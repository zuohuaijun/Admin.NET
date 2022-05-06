using System;
using System.Collections.Generic;

namespace Admin.NET.Core
{
    /// <summary>
    /// 系统配置表种子数据
    /// </summary>
    public class SysConfigSeedData : ISqlSugarEntitySeedData<SysConfig>
    {
        /// <summary>
        /// 种子数据
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SysConfig> HasData()
        {
            return new[]
            {
                new SysConfig{ Id=252885263003800, Name="演示环境", Code="SysDemoEnv", Value="False", SysFlag=YesNoEnum.Y, Remark="演示环境", Order=1, GroupCode="Default", CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
                new SysConfig{ Id=252885263003801, Name="默认密码", Code="SysPassword", Value="123456", SysFlag=YesNoEnum.Y, Remark="默认密码", Order=2, GroupCode="Default", CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
                new SysConfig{ Id=252885263003802, Name="Token过期时间", Code="SysTokenExpire", Value="10080", SysFlag=YesNoEnum.Y, Remark="Token过期时间", Order=3, GroupCode="Default", CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
                new SysConfig{ Id=252885263003803, Name="操作日志", Code="SysOpLog", Value="True", SysFlag=YesNoEnum.Y, Remark="开启操作日志", Order=4, GroupCode="Default", CreateTime=DateTime.Parse("2022-02-10 00:00:00") }
            };
        }
    }
}