using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service
{
    public interface ISysConfigService
    {
        Task AddConfig(AddConfigInput input);

        Task DeleteConfig(DeleteConfigInput input);

        Task<SysConfig> GetConfig([FromQuery] QueryConfigInput input);

        Task<dynamic> GetConfigList();

        Task<dynamic> QueryConfigPageList([FromQuery] ConfigPageInput input);

        Task UpdateConfig(UpdateConfigInput input);

        Task<bool> GetDemoEnvFlag();

        Task<bool> GetCaptchaOpenFlag();

        Task UpdateConfigCache(string code, object value);
    }
}