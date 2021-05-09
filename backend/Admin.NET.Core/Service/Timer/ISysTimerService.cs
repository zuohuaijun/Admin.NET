using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service
{
    public interface ISysTimerService
    {
        Task AddTimer(AddJobInput input);

        void AddTimerJob(AddJobInput input);

        Task DeleteTimer(DeleteJobInput input);

        Task<dynamic> GetTimer([FromQuery] QueryJobInput input);

        Task<dynamic> GetTimerPageList([FromQuery] JobPageInput input);

        void StartTimerJob(AddJobInput input);

        void StopTimerJob(StopJobInput input);

        Task UpdateTimber(UpdateJobInput input);

        void StartTimerJob();
    }
}