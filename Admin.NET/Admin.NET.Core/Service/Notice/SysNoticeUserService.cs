namespace Admin.NET.Core.Service;

/// <summary>
/// 系统通知公告用户
/// </summary>
[ApiDescriptionSettings(Order = 100)]
public class SysNoticeUserService : ISysNoticeUserService, ITransient
{
    private readonly SqlSugarRepository<SysNoticeUser> _sysNoticeUserRep;  // 通知公告用户表仓储

    public SysNoticeUserService(SqlSugarRepository<SysNoticeUser> sysNoticeUserRep)
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
    public async Task Add(long noticeId, List<long> noticeUserIdList, NoticeUserStatusEnum noticeUserStatus)
    {
        var list = new List<SysNoticeUser>();
        noticeUserIdList.ForEach(u =>
        {
            list.Add(new SysNoticeUser
            {
                NoticeId = noticeId,
                UserId = u,
                ReadStatus = noticeUserStatus
            });
        });
        await _sysNoticeUserRep.InsertRangeAsync(list);
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="noticeId"></param>
    /// <param name="noticeUserIdList"></param>
    /// <param name="noticeUserStatus"></param>
    /// <returns></returns>
    public async Task Update(long noticeId, List<long> noticeUserIdList, NoticeUserStatusEnum noticeUserStatus)
    {
        await _sysNoticeUserRep.DeleteAsync(u => u.NoticeId == noticeId);

        await Add(noticeId, noticeUserIdList, noticeUserStatus);
    }

    /// <summary>
    /// 获取通知公告用户列表
    /// </summary>
    /// <param name="noticeId"></param>
    /// <returns></returns>
    public async Task<List<SysNoticeUser>> GetNoticeUserListByNoticeId(long noticeId)
    {
        return await _sysNoticeUserRep.GetListAsync(u => u.NoticeId == noticeId);
    }

    /// <summary>
    /// 设置通知公告读取状态
    /// </summary>
    /// <param name="noticeId"></param>
    /// <param name="userId"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    public async Task Read(long noticeId, long userId, NoticeUserStatusEnum status)
    {
        await _sysNoticeUserRep.UpdateAsync(u => new SysNoticeUser
        {
            ReadStatus = status,
            ReadTime = DateTime.Now
        }, u => u.NoticeId == noticeId && u.UserId == userId);
    }
}