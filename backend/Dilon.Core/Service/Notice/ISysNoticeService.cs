using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dilon.Core.Service.Notice
{
    public interface ISysNoticeService
    {
        Task AddNotice(AddNoticeInput input);
        Task ChangeStatus(ChangeStatusNoticeInput input);
        Task DeleteNotice(DeleteNoticeInput input);
        Task<NoticeDetailOutput> GetNotice([FromQuery] QueryNoticeInput input);
        Task<dynamic> QueryNoticePageList([FromQuery] NoticeInput input);
        Task<dynamic> ReceivedNoticePageList([FromQuery] NoticeInput input);
        Task UpdateNotice(UpdateNoticeInput input);
    }
}