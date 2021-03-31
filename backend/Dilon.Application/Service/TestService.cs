using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dilon.Application
{
    /// <summary>
    /// 事例
    /// </summary>
    public class TestService : ITestService, IDynamicApiController, ITransient
    {
        private readonly ISqlSugarRepository<Test> repository; // 仓储对象：封装简单的CRUD
        private readonly SqlSugarClient db; // 核心对象：拥有完整的SqlSugar全部功能
        public TestService(ISqlSugarRepository<Test> sqlSugarRepository)
        {
            repository = sqlSugarRepository;
            db = (SqlSugarClient)repository.Context;    // 推荐操作
        }



        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <returns></returns>
        [HttpPost("/test/init")]
        public async Task InitData()
        {
            var data = new Test()
            {
                Id = Guid.NewGuid(),
                Name = "测试数据",
                CreateTime = DateTime.Now,
                Text="你好"
            };
           await repository.InsertAsync(data);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        [HttpPost("/test/page")]
        public async Task<List<Test>> GetList()
        {
           return await repository.Entities.ToListAsync();
        }


        public string GetDescription()
        {
            return "Admin.NET";
        }
    }
}
