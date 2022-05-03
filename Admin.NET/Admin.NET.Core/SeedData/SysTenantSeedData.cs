using System;
using System.Collections.Generic;

namespace Admin.NET.Core
{
    /// <summary>
    /// 系统租户表种子数据
    /// </summary>
    public class SysTenantSeedData : ISqlSugarEntitySeedData<SysTenant>
    {
        /// <summary>
        /// 种子数据
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SysTenant> HasData()
        {
            return new[]
            {
                new SysTenant{ Id=142307070918780, Name="租户1", AdminName="admin1", Host="www.dilon.vip", Email="zuohuaijun@163.com", Phone="18020030720", Connection="D://db//1.db", Remark="租户1", CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
                new SysTenant{ Id=142307070918781, Name="租户2", AdminName="admin2", Host="www.dilon.top", Email="515096995@qq.com", Phone="18020030720", Connection="D://db//2.db", Remark="租户2", CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            };
        }
    }
}