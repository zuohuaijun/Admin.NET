using Admin.NET.Core;
using Furion.DatabaseAccessor;
using Furion.DatabaseAccessor.Extensions;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Covid19.Plugin
{
    /// <summary>
    /// 核酸检测项目服务
    /// </summary>
    [ApiDescriptionSettings(ApiGroup.API_GROUP_NAME, Name = "XgTestItem", Order = 100)]
    public class XgTestItemService : IXgTestItemService, IDynamicApiController, ITransient
    {
        private readonly IRepository<XgTestItem> _xgTestItemRep;    // 检测项目表仓储

        public XgTestItemService(IRepository<XgTestItem> xgTestItemRep)
        {
            _xgTestItemRep = xgTestItemRep;
        }

        /// <summary>
        /// 分页获取核酸检测项目
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("/xgTestItem/page"), HttpGet]
        public async Task<dynamic> QueryXgTestItemPageList([FromQuery] XgTestItemInput input)
        {
            var name = !string.IsNullOrEmpty(input.Name?.Trim());
            var testItems = await _xgTestItemRep.DetachedEntities
                                                .Where((name, u => EF.Functions.Like(u.Name, $"%{input.Name.Trim()}%")))
                                                .OrderByDescending(u => u.Id)
                                                .ToPagedListAsync(input.PageNo, input.PageSize);
            return XnPageResult<XgTestItem>.PageResult(testItems);
        }

        /// <summary>
        /// 增加核酸检测项目
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("/xgTestItem/add"), HttpPost]
        public async Task AddXgTestItem(AddXgTestItemInput input)
        {
            var isExist = await _xgTestItemRep.DetachedEntities.AnyAsync(u => u.Name == input.Name);
            if (isExist)
                throw Oops.Oh(ErrorCode.xg1000);

            var xgTestItem = input.Adapt<XgTestItem>();
            await xgTestItem.InsertAsync();
        }

        /// <summary>
        /// 删除核酸检测项目
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("/xgTestItem/delete"), HttpPost]
        public async Task DeleteXgTestItem(DeleteXgTestItemInput input)
        {
            var xgTestItem = await _xgTestItemRep.FirstOrDefaultAsync(u => u.Id == input.Id);
            await xgTestItem.DeleteAsync();
        }

        /// <summary>
        /// 更新核酸检测项目
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("/xgTestItem/edit"), HttpPost]
        public async Task UpdateXgTestItem(UpdateXgTestItemInput input)
        {
            var isExist = await _xgTestItemRep.DetachedEntities.AnyAsync(u => u.Name == input.Name && u.Id != input.Id);
            if (isExist)
                throw Oops.Oh(ErrorCode.xg1000);

            var xgTestItem = input.Adapt<XgTestItem>();
            await xgTestItem.UpdateAsync();
        }

        /// <summary>
        /// 获取核酸检测项目
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("/xgTestItem/detail"), HttpGet]
        public async Task<XgTestItem> GetXgTestItem([FromQuery] QueryXgTestItemInput input)
        {
            return await _xgTestItemRep.DetachedEntities.FirstOrDefaultAsync(u => u.Id == input.Id);
        }
    }
}