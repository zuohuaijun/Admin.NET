using Dilon.Core;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dilon.Application
{
    /// <summary>
    /// 客户公司
    /// </summary>
    [ApiDescriptionSettings(ApiGroupConsts.BUSINESS_CENTER)]
    public class TestAppService
        : BaseService<long, Test, TestEntityOutputDto, TestAddInputDto, TestUpdateInputDto, TestPageListInputDto, MasterDbContextLocator>
        , ITestService
        , IDynamicApiController
        , ITransient
    {
        /// <summary>
        /// 缓存
        /// </summary>
        private readonly IDistributedCache _cache;
        private readonly IRepository<Test> _testCompanyRepository;

        public TestAppService(IDistributedCache cache,
            IRepository<Test> testCompanyRepository)
        {
            _cache = cache;
            _testCompanyRepository = testCompanyRepository;        
        }

        /// <summary>
        /// 缓存添加测试
        /// </summary>
        /// <returns></returns>
        public async Task AddCache()
        {
            await _cache.SetStringAsync("c_", "Hi,I'm jerry");
        }

        /// <summary>
        /// 缓存获取测试
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetCache()
        {
            return await _cache.GetStringAsync("c_");
        }

    }
}