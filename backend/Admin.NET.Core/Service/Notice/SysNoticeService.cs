using Furion.DatabaseAccessor;
using Furion.DatabaseAccessor.Extensions;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service.Notice
{
    /// <summary>
    /// 通知公告服务
    /// </summary>
    [ApiDescriptionSettings(Name = "Notice", Order = 100)]
    public class SysNoticeService : ISysNoticeService, IDynamicApiController, ITransient
    {
        private readonly IRepository<SysNotice> _sysNoticeRep;  // 通知公告表仓储
        private readonly IRepository<SysNoticeUser> _sysNoticeUserRep;  // 通知公告用户表仓储

        private readonly IUserManager _userManager;

        private readonly ISysNoticeUserService _sysNoticeUserService;

        public SysNoticeService(IRepository<SysNotice> sysNoticeRep,
                                IRepository<SysNoticeUser> sysNoticeUserRep,
                                IUserManager userManager,
                                ISysNoticeUserService sysNoticeUserService)
        {
            _sysNoticeRep = sysNoticeRep;
            _sysNoticeUserRep = sysNoticeUserRep;
            _userManager = userManager;
            _sysNoticeUserService = sysNoticeUserService;
        }

        /// <summary>
        /// 分页查询通知公告
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysNotice/page")]
        public async Task<dynamic> QueryNoticePageList([FromQuery] NoticePageInput input)
        {
            var searchValue = !string.IsNullOrEmpty(input.SearchValue?.Trim());
            var notices = await _sysNoticeRep.DetachedEntities
                                             .Where(searchValue, u => EF.Functions.Like(u.Title, $"%{input.SearchValue.Trim()}%") ||
                                                                      EF.Functions.Like(u.Content, $"%{input.SearchValue.Trim()}%"))
                                             .Where(input.Type > 0, u => u.Type == input.Type)
                                             .Where(u => u.Status != NoticeStatus.DELETED)
                                             .ToPagedListAsync(input.PageNo, input.PageSize);
            return XnPageResult<SysNotice>.PageResult(notices);
        }

        /// <summary>
        /// 增加通知公告
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysNotice/add")]
        public async Task AddNotice(AddNoticeInput input)
        {
            if (input.Status != NoticeStatus.DRAFT && input.Status != NoticeStatus.PUBLIC)
                throw Oops.Oh(ErrorCode.D7000);

            var notice = input.Adapt<SysNotice>();
            await UpdatePublicInfo(notice);
            // 如果是发布，则设置发布时间
            if (input.Status == NoticeStatus.PUBLIC)
                notice.PublicTime = DateTimeOffset.Now;
            var newItem = await notice.InsertNowAsync();

            // 通知到的人
            var noticeUserIdList = input.NoticeUserIdList;
            var noticeUserStatus = NoticeUserStatus.UNREAD;
            await _sysNoticeUserService.Add(newItem.Entity.Id, noticeUserIdList, noticeUserStatus);
        }

        /// <summary>
        /// 删除通知公告
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysNotice/delete")]
        public async Task DeleteNotice(DeleteNoticeInput input)
        {
            var notice = await _sysNoticeRep.FirstOrDefaultAsync(u => u.Id == input.Id);
            if (notice.Status != NoticeStatus.DRAFT && notice.Status != NoticeStatus.CANCEL) // 只能删除草稿和撤回的
                throw Oops.Oh(ErrorCode.D7001);

            //notice.Status = (int)NoticeStatus.DELETED;
            await notice.DeleteAsync();
        }

        /// <summary>
        /// 更新通知公告
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysNotice/edit")]
        public async Task UpdateNotice(UpdateNoticeInput input)
        {
            if (input.Status != NoticeStatus.DRAFT && input.Status != NoticeStatus.PUBLIC)
                throw Oops.Oh(ErrorCode.D7000);

            //  非草稿状态
            if (input.Status != NoticeStatus.DRAFT)
                throw Oops.Oh(ErrorCode.D7002);

            var notice = input.Adapt<SysNotice>();
            if (input.Status == NoticeStatus.PUBLIC)
            {
                notice.PublicTime = DateTimeOffset.Now;
                await UpdatePublicInfo(notice);
            }
            await notice.UpdateAsync();

            // 通知到的人
            var noticeUserIdList = input.NoticeUserIdList;
            var noticeUserStatus = NoticeUserStatus.UNREAD;
            await _sysNoticeUserService.Update(input.Id, noticeUserIdList, noticeUserStatus);
        }

        /// <summary>
        /// 获取通知公告详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysNotice/detail")]
        public async Task<NoticeDetailOutput> GetNotice([FromQuery] QueryNoticeInput input)
        {
            var notice = await _sysNoticeRep.FirstOrDefaultAsync(u => u.Id == input.Id);

            // 获取通知到的用户
            var noticeUserList = await _sysNoticeUserService.GetNoticeUserListByNoticeId(input.Id);
            var noticeUserIdList = new List<string>();
            var noticeUserReadInfoList = new List<NoticeUserRead>();
            if (noticeUserList != null)
            {
                noticeUserList.ForEach(u =>
                {
                    noticeUserIdList.Add(u.UserId.ToString());
                    var noticeUserRead = new NoticeUserRead
                    {
                        UserId = u.UserId,
                        UserName = _userManager.Name,
                        ReadStatus = u.ReadStatus,
                        ReadTime = u.ReadTime
                    };
                    noticeUserReadInfoList.Add(noticeUserRead);
                });
            }
            var noticeResult = notice.Adapt<NoticeDetailOutput>();
            noticeResult.NoticeUserIdList = noticeUserIdList;
            noticeResult.NoticeUserReadInfoList = noticeUserReadInfoList;
            // 如果该条通知公告为已发布，则将当前用户的该条通知公告设置为已读
            if (notice.Status == NoticeStatus.PUBLIC)
                await _sysNoticeUserService.Read(notice.Id, _userManager.UserId, NoticeUserStatus.READ);
            return noticeResult;
        }

        /// <summary>
        /// 修改通知公告状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysNotice/changeStatus")]
        public async Task ChangeStatus(ChangeStatusNoticeInput input)
        {
            // 状态应为撤回或删除或发布
            if (input.Status != NoticeStatus.CANCEL && input.Status != NoticeStatus.DELETED && input.Status != NoticeStatus.PUBLIC)
                throw Oops.Oh(ErrorCode.D7000);

            var notice = await _sysNoticeRep.FirstOrDefaultAsync(u => u.Id == input.Id);
            notice.Status = input.Status;

            if (input.Status == NoticeStatus.CANCEL)
            {
                notice.CancelTime = DateTimeOffset.Now;
            }
            else if (input.Status == NoticeStatus.PUBLIC)
            {
                notice.PublicTime = DateTimeOffset.Now;
            }
            await notice.UpdateAsync();
        }

        /// <summary>
        /// 获取接收的通知公告
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysNotice/received")]
        public async Task<dynamic> ReceivedNoticePageList([FromQuery] NoticePageInput input)
        {
            var searchValue = !string.IsNullOrEmpty(input.SearchValue?.Trim());
            var notices = await _sysNoticeRep.DetachedEntities.Join(_sysNoticeUserRep.DetachedEntities, u => u.Id, e => e.NoticeId, (u, e) => new { u, e })
                                             .Where(u => u.e.UserId == _userManager.UserId)
                                             .Where(searchValue, u => EF.Functions.Like(u.u.Title, $"%{input.SearchValue.Trim()}%") ||
                                                                      EF.Functions.Like(u.u.Content, $"%{input.SearchValue.Trim()}%"))
                                             .Where(input.Type > 0, u => u.u.Type == input.Type)
                                             .Where(u => u.u.Status != NoticeStatus.DELETED)
                                             .Select(u => u.u.Adapt(u.e.Adapt<NoticeReceiveOutput>()))
                                             .ToPagedListAsync(input.PageNo, input.PageSize);
            return XnPageResult<NoticeReceiveOutput>.PageResult(notices);
        }

        /// <summary>
        /// 更新发布信息
        /// </summary>
        /// <param name="notice"></param>
        [NonAction]
        private async Task UpdatePublicInfo(SysNotice notice)
        {
            var emp = await _userManager.GetUserEmpInfo(_userManager.UserId);
            notice.PublicUserId = _userManager.UserId;
            notice.PublicUserName = _userManager.Name;
            notice.PublicOrgId = emp.OrgId;
            notice.PublicOrgName = emp.OrgName;
        }
    }
}