using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dilon.Core.Service
{
    public interface ISysTimerService
    {
        Task AddJob(JobInput input);
        Task DeleteJob(DeleteJobInput input);
        Task<dynamic> GetJobPageList([FromQuery] JobInput input);
        Task<dynamic> GetTimer([FromQuery] JobInput input);
        Task StopScheduleJobAsync(JobInput input);
        Task TriggerJobAsync(JobInput input);
        Task UpdateJob(UpdateJobInput input);
    }
}