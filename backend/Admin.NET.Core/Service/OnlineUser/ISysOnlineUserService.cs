using System.Threading.Tasks;

namespace Admin.NET.Core.Service
{
    public interface ISysOnlineUserService
    {
        Task<dynamic> List();
    }
}