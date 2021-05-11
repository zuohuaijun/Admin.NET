using Admin.NET.Core;
using Furion.DatabaseAccessor;
using Furion.DatabaseAccessor.Extensions;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Yitter.IdGenerator;

namespace Covid19.Plugin
{
    /// <summary>
    /// 样本采集服务
    /// </summary>
    [ApiDescriptionSettings(ApiGroup.API_GROUP_NAME, Name = "XgCollector", Order = 100)]
    public class XgCollectorService : IXgCollectorService, IDynamicApiController, ITransient
    {
        private readonly IRepository<XgCollector> _xgCollectorRep;    // 标本采集人员表仓储

        public XgCollectorService(IRepository<XgCollector> xgCollectorRep)
        {
            _xgCollectorRep = xgCollectorRep;
        }

        /// <summary>
        /// 分页获取样本采集人员
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("/xgCollector/page"), HttpGet]
        public async Task<dynamic> QueryXgCollectorPageList([FromQuery] XgCollectorInput input)
        {
            var name = !string.IsNullOrEmpty(input.Name?.Trim());
            var idNumber = !string.IsNullOrEmpty(input.IdNumber?.Trim());
            var phone = !string.IsNullOrEmpty(input.Phone?.Trim());
            var collectors = await _xgCollectorRep.DetachedEntities
                                                  .Where((name, u => EF.Functions.Like(u.Name, $"%{input.Name.Trim()}%")),
                                                         (idNumber, u => EF.Functions.Like(u.IdNumber, $"%{input.IdNumber.Trim()}%")),
                                                         (phone, u => EF.Functions.Like(u.Phone, $"%{input.Phone.Trim()}%")))
                                                  .OrderByDescending(u => u.Id)
                                                  .ToPagedListAsync(input.PageNo, input.PageSize);
            return XnPageResult<XgCollector>.PageResult(collectors);
        }

        /// <summary>
        /// 增加样本采集人员
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("/xgCollector/add"), HttpPost]
        public async Task AddXgCollector(AddXgCollectorInput input)
        {
            var xgCollector = input.Adapt<XgCollector>();
            xgCollector.CollectionTime = DateTimeOffset.Now;
            xgCollector.Number = YitIdHelper.NextId().ToString();
            var newItem = await xgCollector.InsertNowAsync();

            var xgTest = new XgTest
            {
                CollectorId = newItem.Entity.Id
            };
            await xgTest.InsertAsync();
        }

        /// <summary>
        /// 删除样本采集人员
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("/xgCollector/delete"), HttpPost]
        public async Task DeleteXgCollector(DeleteXgCollectorInput input)
        {
            var xgCollector = await _xgCollectorRep.FirstOrDefaultAsync(u => u.Id == input.Id);
            await xgCollector.DeleteAsync();
        }

        /// <summary>
        /// 更新样本采集人员
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("/xgCollector/edit"), HttpPost]
        public async Task UpdateXgCollector(UpdateXgCollectorInput input)
        {
            var xgCollector = input.Adapt<XgCollector>();
            await xgCollector.UpdateExcludeAsync(new[] { nameof(xgCollector.CollectionTime), nameof(xgCollector.Number) }, true);
        }

        /// <summary>
        /// 获取样本采集人员
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("/xgCollector/detail"), HttpGet]
        public async Task<XgCollector> GetXgCollector([FromQuery] QueryXgCollectorInput input)
        {
            return await _xgCollectorRep.DetachedEntities.FirstOrDefaultAsync(u => u.Id == input.Id);
        }
    }
}
