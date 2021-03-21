using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dilon.Core.Service.Notice
{
    public interface ISysNoticeUserService
    {
        Task Add(long noticeId, List<long> noticeUserIdList, int noticeUserStatus);
        Task<List<SysNoticeUser>> GetNoticeUserListByNoticeId(long noticeId);
        Task Read(long noticeId, long userId, int status);
        Task Update(long noticeId, List<long> noticeUserIdList, int noticeUserStatus);
    }
}