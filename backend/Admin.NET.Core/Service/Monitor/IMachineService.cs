using System.Threading.Tasks;

namespace Admin.NET.Core.Service
{
    public interface IMachineService
    {
        Task<dynamic> GetMachineBaseInfo();

        Task<dynamic> GetMachineNetWorkInfo();

        Task<dynamic> GetMachineUseInfo();
    }
}