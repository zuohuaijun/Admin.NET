using System;
using System.Collections.Generic;

namespace Admin.NET.Core.SeedData
{
    /// <summary>
    /// 系统行政区域种子数据
    /// </summary>
    public class SysDistrictSeedData : ISqlSugarEntitySeedData<SysDistrict>
    {
        /// <summary>
        /// 种子数据
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SysDistrict> HasData()
        {
            return new[]
            {
                new SysDistrict{ Id=243848612100001, Pid=0, Name="行政区",Value="district", Code="1001", CreateTime=DateTime.Parse("2022-05-30 00:00:00"), Remark="行政区"},
                new SysDistrict{ Id=243848612100002, Pid=243848612100001, Name="北京市",Value="110000", Code="1001001", CreateTime=DateTime.Parse("2022-05-30 00:00:00"), Remark="北京市"},
                new SysDistrict{ Id=243848612100003, Pid=243848612100002, Name="东城区",Value="110101", Code="1001001001", CreateTime=DateTime.Parse("2022-05-30 00:00:00"), Remark="东城区"},
            };
        }
    }
}
