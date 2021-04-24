using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dilon.Core.Service
{
    public interface ISysTimerService
    {
        Task AddTimer(JobInput input);
        void AddTimerJob(JobInput input);
        Task DeleteTimer(DeleteJobInput input);
        Task<dynamic> GetTimer([FromQuery] QueryJobInput input);
        Task<dynamic> GetTimerPageList([FromQuery] JobInput input);
        void StartTimerJob(JobInput input);
        void StopTimerJob(JobInput input);
        Task UpdateTimber(UpdateJobInput input);
        void StartTimerJob();
    }
}