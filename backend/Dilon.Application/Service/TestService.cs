using Dilon.Core;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.Snowflake;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dilon.Application
{
    /// <summary>
    /// 业务服务及集成SqlSugar用法事例
    /// </summary>
    public class TestService : ITestService, IDynamicApiController, ITransient
    {
        //private readonly ISqlSugarRepository<Test> _testRep;
        //private readonly SqlSugarClient _db; // SqlSugar对象

        public TestService(/*ISqlSugarRepository<Test> sqlSugarRep*/)
        {
            //_testRep = sqlSugarRep;
            //_db = (SqlSugarClient)_testRep.Context;
        }

        /// <summary>
        /// 测试方法
        /// </summary>
        /// <returns></returns>
        [HttpPost("/test")]
        public string GetDescription()
        {
            return "Admin.NET";
        }

        ///// <summary>
        ///// 增加数据
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost("/test/add")]
        //public async Task AddTestAsync()
        //{
        //    var test = new Test()
        //    {
        //        Id = IDGenerator.NextId(),
        //        Name = "Admin.NET",
        //        Age = 1,
        //        CreateTime = DateTimeOffset.Now
        //    };
        //    await _testRep.InsertAsync(test);
        //    // _db.Insertable(test).ExecuteCommand();
        //}

        ///// <summary>
        ///// 查询所有
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost("/test/page")]
        //public async Task<List<Test>> GetTestListAsync()
        //{
        //    return await _testRep.Entities.ToListAsync();
        //}

        ///// <summary>
        ///// 查询系统用户
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost("/test/userPage")]
        //public async Task<dynamic> GetUserListAsync()
        //{
        //    return await _db.Queryable<SysUser>().ToListAsync();
        //}
    }
}
