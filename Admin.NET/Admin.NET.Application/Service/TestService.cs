using Admin.NET.Application.Const;
using Admin.NET.Application.Entity;
using Admin.NET.Core;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Admin.NET.Application.Serice
{
    /// <summary>
    /// 自己业务服务
    /// </summary>
    [ApiDescriptionSettings(TestConst.GroupName, Name = "XXX模块", Order = 200)]
    public class TestService : IDynamicApiController, ITransient
    {
        private readonly SqlSugarRepository<Test> _testRep;

        public TestService(SqlSugarRepository<Test> testRep)
        {
            _testRep = testRep;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("/test/list")]
        public async Task<List<Test>> GetTestList()
        {
            return await _testRep.GetListAsync();
        }
    }
}
