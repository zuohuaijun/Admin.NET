namespace Admin.NET.Core.Service;

public interface ISysNoticeUserService
{
    Task Add(long noticeId, List<long> noticeUserIdList, NoticeUserStatusEnum noticeUserStatus);

    Task<List<SysNoticeUser>> GetNoticeUserListByNoticeId(long noticeId);

    Task Read(long noticeId, long userId, NoticeUserStatusEnum status);

    Task Update(long noticeId, List<long> noticeUserIdList, NoticeUserStatusEnum noticeUserStatus);
}