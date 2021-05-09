using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service.Notice
{
    public interface ISysNoticeService
    {
        Task AddNotice(AddNoticeInput input);

        Task ChangeStatus(ChangeStatusNoticeInput input);

        Task DeleteNotice(DeleteNoticeInput input);

        Task<NoticeDetailOutput> GetNotice([FromQuery] QueryNoticeInput input);

        Task<dynamic> QueryNoticePageList([FromQuery] NoticePageInput input);

        Task<dynamic> ReceivedNoticePageList([FromQuery] NoticePageInput input);

        Task UpdateNotice(UpdateNoticeInput input);
    }
}