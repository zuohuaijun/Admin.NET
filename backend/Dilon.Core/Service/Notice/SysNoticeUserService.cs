using Furion.DatabaseAccessor;
using Furion.DatabaseAccessor.Extensions;
using Furion.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dilon.Core.Service.Notice
{
    /// <summary>
    /// 通知公告用户
    /// </summary>
    public class SysNoticeUserService : ISysNoticeUserService, ITransient
    {
        private readonly IRepository<SysNoticeUser> _sysNoticeUserRep;  // 通知公告用户表仓储 

        public SysNoticeUserService(IRepository<SysNoticeUser> sysNoticeUserRep)
        {
            _sysNoticeUserRep = sysNoticeUserRep;
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="noticeId"></param>
        /// <param name="noticeUserIdList"></param>
        /// <param name="noticeUserStatus"></param>
        /// <returns></returns>
        public Task Add(long noticeId, List<long> noticeUserIdList, int noticeUserStatus)
        {
            var noticeUsers = new List<Task<SysNoticeUser>>();
            noticeUserIdList.ForEach(u =>
            {
                new SysNoticeUser
                {
                    NoticeId = noticeId,
                    UserId = u,
                    ReadStatus = noticeUserStatus
                }.Insert();
            });
            return Task.CompletedTask;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="noticeId"></param>
        /// <param name="noticeUserIdList"></param>
        /// <param name="noticeUserStatus"></param>
        /// <returns></returns>
        public async Task Update(long noticeId, List<long> noticeUserIdList, int noticeUserStatus)
        {
            var noticeUsers = await _sysNoticeUserRep.Where(u => u.NoticeId == noticeId).ToListAsync();
            noticeUsers.ForEach(u =>
            {
                u.Delete();
            });

            await Add(noticeId, noticeUserIdList, noticeUserStatus);
        }

        /// <summary>
        /// 获取通知公告用户列表
        /// </summary>
        /// <param name="noticeId"></param>
        /// <returns></returns>
        public async Task<List<SysNoticeUser>> GetNoticeUserListByNoticeId(long noticeId)
        {
            return await _sysNoticeUserRep.Where(u => u.NoticeId == noticeId).ToListAsync();
        }

        /// <summary>
        /// 设置通知公告读取状态
        /// </summary>
        /// <param name="noticeId"></param>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task Read(long noticeId, long userId, int status)
        {
            var noticeUser = await _sysNoticeUserRep.FirstOrDefaultAsync(u => u.NoticeId == noticeId && u.UserId == userId);
            if (noticeUser != null)
            {
                noticeUser.ReadStatus = status;
                noticeUser.ReadTime = DateTimeOffset.Now;
                await noticeUser.UpdateAsync();
            }
        }
    }
}
