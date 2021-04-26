using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dilon.Core.Service
{
    public interface ISysConfigService
    {
        Task AddConfig(AddConfigInput input);

        Task DeleteConfig(DeleteConfigInput input);

        Task<SysConfig> GetConfig([FromQuery] QueryConfigInput input);

        Task<dynamic> GetConfigList([FromQuery] ConfigInput input);

        Task<dynamic> QueryConfigPageList([FromQuery] ConfigInput input);

        Task UpdateConfig(UpdateConfigInput input);

        Task<bool> GetDemoEnvFlag();

        Task<bool> GetCaptchaOpenFlag();

        Task UpdateConfigCache(string code, object value);
    }
}