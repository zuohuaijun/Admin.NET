using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dilon.Core.Service
{
    public interface ISysAppService
    {
        Task AddApp(AddAppInput input);
        Task DeleteApp(DeleteAppInput input);
        Task<SysApp> GetApp([FromQuery] QueryAppInput input);
        Task<dynamic> GetAppList([FromQuery] AppInput input);
        Task<dynamic> GetLoginApps(long userId);
        Task<dynamic> QueryAppPageList([FromQuery] AppInput input);
        Task SetAsDefault(SetDefaultAppInput input);
        Task UpdateApp(UpdateAppInput input);
        Task ChangeUserAppStatus(UpdateAppInput input);
    }
}