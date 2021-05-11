using Admin.NET.Core;
using Furion;
using Furion.DatabaseAccessor;
using Furion.DatabaseAccessor.Extensions;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Covid19.Plugin
{
    /// <summary>
    /// 核酸检测服务
    /// </summary>
    [ApiDescriptionSettings(ApiGroup.API_GROUP_NAME, Name = "XgTest", Order = 100)]
    public class XgTestService : IXgTestService, IDynamicApiController, ITransient 
    {
        private readonly IRepository<XgTest> _xgTestRep;    // 核酸检测表仓储

        private readonly IUserManager _userManager;

        public XgTestService(IRepository<XgTest> xgTestRep, IUserManager userManager)
        {
            _xgTestRep = xgTestRep;
            _userManager = userManager;
        }

        /// <summary>
        /// 分页获取核酸检测
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("/xgTest/page"), HttpGet]
        public async Task<dynamic> QueryXgTestPageList([FromQuery] XgTestInput input)
        {
            var name = !string.IsNullOrEmpty(input.Name?.Trim());
            var idNumber = !string.IsNullOrEmpty(input.IdNumber?.Trim());
            var xgTests = await _xgTestRep.DetachedEntities
                                          .Where((name, u => EF.Functions.Like(u.Collector.Name, $"%{input.Name.Trim()}%")),
                                                 (idNumber, u => EF.Functions.Like(u.Collector.IdNumber, $"%{input.IdNumber.Trim()}%")))
                                          .OrderByDescending(u => u.Id)
                                          .Select(u => new XgTestOutput
                                          {
                                              Id = u.Id,
                                              Number = u.Collector.Number,
                                              Name = u.Collector.Name,
                                              Sex = u.Collector.Sex,
                                              IdNumber = u.Collector.IdNumber,
                                              Phone = u.Collector.Phone,
                                              Birthday = u.Collector.Birthday,
                                              CollectionTime = u.Collector.CollectionTime,
                                              SiteId = u.Collector.SiteId,
                                              XgOrflab = u.XgOrflab,
                                              XgN = u.XgN,
                                              IgG = u.IgG,
                                              IgM = u.IgM,
                                              TestDoctor = u.TestDoctor,
                                              TestTime = u.TestTime,
                                              AuditDoctor = u.AuditDoctor,
                                              AuditTime = u.AuditTime                                              
                                          })
                                          .ToPagedListAsync(input.PageNo, input.PageSize);
            return XnPageResult<XgTestOutput>.PageResult(xgTests);
        }

        /// <summary>
        /// 批量设置阴性(1阴性 2阳性)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("/xgTest/updateNegative"), HttpPost]
        public async Task UpdateNegative(IdsInput input)
        {
            var xgTests = await _xgTestRep.Where(u => input.Ids.Contains(u.Id) && u.XgOrflab < 1).ToListAsync();
            var userName = _userManager.Account;
            var time = DateTimeOffset.Now;
            xgTests.ForEach(u =>
            {
                u.XgOrflab = 1;
                u.XgN = 1;
                u.TestDoctor = userName;
                u.TestTime = time;
            });
        }

        /// <summary>
        /// 更新检测结果（设置阴阳性和血检结果）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("/xgTest/updateTestResult"), HttpPost]
        public async Task UpdateTestResult(XgTestInput input)
        {
            var isExist = await _xgTestRep.DetachedEntities.AnyAsync(u => u.Id == input.Id);
            if (!isExist)
                throw Oops.Oh(ErrorCode.xg1001);
            var newItem = input.Adapt<XgTest>();
            newItem.TestDoctor = App.GetService<IUserManager>().Account;
            newItem.TestTime = DateTimeOffset.Now;
            await newItem.UpdateIncludeAsync(new[] { nameof(XgTest.XgOrflab), nameof(XgTest.XgN), nameof(XgTest.IgG), nameof(XgTest.IgM), nameof(XgTest.TestDoctor), nameof(XgTest.TestTime) }, true);
        }

        /// <summary>
        /// 审核检测数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("/xgTest/checkTestResult"), HttpPost]
        public async Task CheckTestResult(IdsInput input)
        {
            var xgTests = await _xgTestRep.Where(u => input.Ids.Contains(u.Id) && string.IsNullOrEmpty(u.AuditDoctor) && u.XgOrflab > 0).ToListAsync();
            var userName = _userManager.Account;
            var time = DateTimeOffset.Now;
            xgTests.ForEach(u =>
            {
                u.AuditDoctor = userName;
                u.AuditTime = time;
            });
        }
    }
}
