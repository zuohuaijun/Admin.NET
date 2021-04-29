using System.Threading.Tasks;
using Dilon.Core.Hubs;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Dilon.Application
{
    /// <summary>
    /// 业务服务及集成SqlSugar用法事例
    /// </summary>
    [ApiDescriptionSettings("自己的业务", Name = "Test", Order = 100)]
    public class TestService : ITestService, IDynamicApiController, ITransient
    {
        //private readonly ISqlSugarRepository<Test> _testRep;
        //private readonly SqlSugarClient _db; // SqlSugar对象

        private readonly IHubContext<ChatHub> _chatHub;

        public TestService(IHubContext<ChatHub> chatHub /*ISqlSugarRepository<Test> sqlSugarRep*/)
        {
            _chatHub = chatHub;
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
        
        /// <summary>
        /// 发送消息到客户端
        /// </summary>
        [AllowAnonymous]
        public async Task SendMessage()
        {
            await _chatHub.Clients.All.SendAsync("ReceiveMessage", "这是一条针对所有客户端的消息");
        }
    }
}