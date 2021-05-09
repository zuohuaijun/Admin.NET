using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service
{
    public interface ISysAppService
    {
        Task AddApp(AddAppInput input);

        Task DeleteApp(BaseId input);

        Task<SysApp> GetApp([FromQuery] QueryAppInput input);

        Task<dynamic> GetAppList();

        Task<dynamic> GetLoginApps(long userId);

        Task<dynamic> QueryAppPageList([FromQuery] AppPageInput input);

        Task SetAsDefault(SetDefaultAppInput input);

        Task UpdateApp(UpdateAppInput input);

        Task ChangeUserAppStatus(ChangeUserAppStatusInput input);
    }
}