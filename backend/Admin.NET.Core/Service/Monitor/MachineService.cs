using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service
{
    /// <summary>
    /// 服务器信息服务
    /// </summary>
    [AllowAnonymous]
    [ApiDescriptionSettings(Name = "Machine", Order = 100)]
    public class MachineService : IMachineService, IDynamicApiController, ITransient
    {
        public MachineService()
        {
        }

        /// <summary>
        /// 获取服务器资源信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("/sysMachine/use")]
        public async Task<dynamic> GetMachineUseInfo()
        {
            var useInfo = MachineUtil.GetMachineUseInfo();
            return await Task.FromResult(useInfo);
        }

        /// <summary>
        /// 获取服务器基本参数
        /// </summary>
        /// <returns></returns>
        [HttpGet("/sysMachine/base")]
        public async Task<dynamic> GetMachineBaseInfo()
        {
            return await MachineUtil.GetMachineBaseInfo();
        }

        /// <summary>
        /// 动态获取网络信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("/sysMachine/network")]
        public async Task<dynamic> GetMachineNetWorkInfo()
        {
            var baseInfo = MachineUtil.GetMachineNetWorkInfo();
            return await Task.FromResult(baseInfo);
        }
    }
}