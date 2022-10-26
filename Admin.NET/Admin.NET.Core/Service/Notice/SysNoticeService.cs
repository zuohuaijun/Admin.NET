namespace Admin.NET.Core.Service;

/// <summary>
/// 系统通知公告服务
/// </summary>
[ApiDescriptionSettings(Order = 100)]
public class SysNoticeService : ISysNoticeService, IDynamicApiController, ITransient
{
    private readonly UserManager _userManager;

    private readonly SqlSugarRepository<SysUser> _sysUserRep;
    private readonly SqlSugarRepository<SysNotice> _sysNoticeRep;

    private readonly SysOnlineUserService _sysOnlineUserService;
    private readonly ISysNoticeUserService _sysNoticeUserService;

    public SysNoticeService(
        UserManager userManager,
        SqlSugarRepository<SysUser> sysUserRep,
        SqlSugarRepository<SysNotice> sysNoticeRep,
        SysOnlineUserService sysOnlineUserService,
        ISysNoticeUserService sysNoticeUserService)
    {
        _userManager = userManager;
        _sysUserRep = sysUserRep;
        _sysNoticeRep = sysNoticeRep;
        _sysOnlineUserService = sysOnlineUserService;
        _sysNoticeUserService = sysNoticeUserService;
    }

    /// <summary>
    /// 分页查询通知公告
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet("/sysNotice/page")]
    public async Task<dynamic> QueryNoticePageList([FromQuery] NoticeInput input)
    {
        return await _sysNoticeRep.Context.Queryable<SysNotice>()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Title), u => u.Title.Contains(input.Title.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Content), u => u.Content.Contains(input.Content.Trim()))
            .WhereIF(input.Type > 0, u => u.Type == input.Type)
            .Where(u => u.Status != NoticeStatusEnum.DELETED)
            .ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加通知公告
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/sysNotice/add")]
    public async Task AddNotice(AddNoticeInput input)
    {
        if (input.Status != NoticeStatusEnum.DRAFT && input.Status != NoticeStatusEnum.PUBLIC)
            throw Oops.Oh(ErrorCodeEnum.D7000);

        var notice = input.Adapt<SysNotice>();
        await UpdatePublicInfo(notice);
        // 如果是发布，则设置发布时间
        if (input.Status == NoticeStatusEnum.PUBLIC)
            notice.PublicTime = DateTime.Now;
        var newItem = await _sysNoticeRep.AsInsertable(notice).ExecuteReturnEntityAsync();

        // 通知到的人
        var noticeUserIdList = input.NoticeUserIdList;
        var noticeUserStatus = NoticeUserStatusEnum.UNREAD;
        await _sysNoticeUserService.Add(newItem.Id, noticeUserIdList, noticeUserStatus);
        if (newItem.Status == NoticeStatusEnum.PUBLIC)
        {
            await _sysOnlineUserService.AppendNotice(newItem, noticeUserIdList);
        }
    }

    /// <summary>
    /// 删除通知公告
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/sysNotice/delete")]
    public async Task DeleteNotice(DeleteNoticeInput input)
    {
        var notice = await _sysNoticeRep.GetFirstAsync(u => u.Id == input.Id);
        if (notice.Status != NoticeStatusEnum.DRAFT && notice.Status != NoticeStatusEnum.CANCEL) // 只能删除草稿和撤回的
            throw Oops.Oh(ErrorCodeEnum.D7001);

        await _sysNoticeRep.DeleteAsync(notice);
    }

    /// <summary>
    /// 更新通知公告
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/sysNotice/edit")]
    public async Task UpdateNotice(UpdateNoticeInput input)
    {
        if (input.Status != NoticeStatusEnum.DRAFT && input.Status != NoticeStatusEnum.PUBLIC)
            throw Oops.Oh(ErrorCodeEnum.D7000);

        //  非草稿状态
        if (input.Status != NoticeStatusEnum.DRAFT)
            throw Oops.Oh(ErrorCodeEnum.D7002);

        var notice = input.Adapt<SysNotice>();
        if (input.Status == NoticeStatusEnum.PUBLIC)
        {
            notice.PublicTime = DateTime.Now;
            await UpdatePublicInfo(notice);
        }
        await _sysNoticeRep.UpdateAsync(notice);

        // 通知到的人
        var noticeUserIdList = input.NoticeUserIdList;
        var noticeUserStatus = NoticeUserStatusEnum.UNREAD;
        await _sysNoticeUserService.Update(input.Id, noticeUserIdList, noticeUserStatus);
        if (notice.Status == NoticeStatusEnum.PUBLIC)
        {
            await _sysOnlineUserService.AppendNotice(notice, noticeUserIdList);
        }
    }

    /// <summary>
    /// 获取通知公告详情
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet("/sysNotice/detail")]
    public async Task<NoticeDetailOutput> GetNotice([FromQuery] QueryNoticeInput input)
    {
        var notice = await _sysNoticeRep.GetFirstAsync(u => u.Id == input.Id);

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
                    UserName = _userManager.UserName,
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
        if (notice.Status == NoticeStatusEnum.PUBLIC)
            await _sysNoticeUserService.Read(notice.Id, _userManager.UserId, NoticeUserStatusEnum.READ);
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
        if (input.Status != NoticeStatusEnum.CANCEL && input.Status != NoticeStatusEnum.DELETED && input.Status != NoticeStatusEnum.PUBLIC)
            throw Oops.Oh(ErrorCodeEnum.D7000);

        var notice = await _sysNoticeRep.GetFirstAsync(u => u.Id == input.Id);
        notice.Status = input.Status;

        if (input.Status == NoticeStatusEnum.CANCEL)
        {
            notice.CancelTime = DateTime.Now;
        }
        else if (input.Status == NoticeStatusEnum.PUBLIC)
        {
            notice.PublicTime = DateTime.Now;
        }
        await _sysNoticeRep.UpdateAsync(notice);
        if (notice.Status == NoticeStatusEnum.PUBLIC)
        {
            // 获取通知到的用户
            var noticeUserList = await _sysNoticeUserService.GetNoticeUserListByNoticeId(input.Id);
            await _sysOnlineUserService.AppendNotice(notice, noticeUserList.Select(m => m.UserId).ToList());
        }
    }

    /// <summary>
    /// 获取接收的通知公告
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet("/sysNotice/received")]
    public async Task<dynamic> ReceivedNoticePageList([FromQuery] NoticeInput input)
    {
        return await _sysNoticeRep.Context.Queryable<SysNotice, SysNoticeUser>((n, u) => new JoinQueryInfos(JoinType.Inner, n.Id == u.NoticeId))
          .Where((n, u) => u.UserId == _userManager.UserId)
          .WhereIF(!string.IsNullOrWhiteSpace(input.Title), u => u.Title.Contains(input.Title.Trim()))
          .WhereIF(!string.IsNullOrWhiteSpace(input.Content), u => u.Title.Contains(input.Content.Trim()))
          .WhereIF(input.Type > 0, (n, u) => n.Type == input.Type)
          .Select<NoticeReceiveOutput>()
          .ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 未处理消息
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet("/sysNotice/unread")]
    public async Task<dynamic> UnReadNoticeList([FromQuery] NoticeInput input)
    {
        var dic = typeof(NoticeTypeEnum).EnumToList();
        var notices = await _sysNoticeRep.Context.Queryable<SysNotice, SysNoticeUser>((n, u) => new JoinQueryInfos(JoinType.Inner, n.Id == u.NoticeId))
            .Where((n, u) => u.UserId == _userManager.UserId && u.ReadStatus == NoticeUserStatusEnum.UNREAD)
            .PartitionBy(n => n.Type).OrderBy(n => n.CreateTime, OrderByType.Desc)
            .Take(input.PageSize).Select<NoticeReceiveOutput>().ToListAsync();
        var count = await _sysNoticeRep.Context.Queryable<SysNotice, SysNoticeUser>((n, u) => new JoinQueryInfos(JoinType.Inner, n.Id == u.NoticeId))
            .Where((n, u) => u.UserId == _userManager.UserId && u.ReadStatus == NoticeUserStatusEnum.UNREAD).CountAsync();

        var noticeClays = new List<dynamic>();
        int index = 0;
        foreach (var item in dic)
        {
            noticeClays.Add(new
            {
                Index = index++,
                Key = item.Describe,
                Value = item.Value,
                NoticeData = notices.Where(m => m.Type == item.Value).ToList()
            });
        }
        return new
        {
            Rows = noticeClays,
            TotalRows = count
        };
    }

    /// <summary>
    /// 更新发布信息
    /// </summary>
    /// <param name="notice"></param>
    [NonAction]
    private async Task UpdatePublicInfo(SysNotice notice)
    {
        var user = await _sysUserRep.GetFirstAsync(u => u.Id == _userManager.UserId);
        notice.PublicUserId = _userManager.UserId;
        notice.PublicUserName = _userManager.UserName;
        notice.PublicOrgId = user.OrgId;
        // notice.PublicOrgName = user.OrgName;
    }
}