using System.Threading.Tasks;

namespace Dilon.Core.Service
{
    public interface ISysOnlineUserService
    {
        Task<dynamic> List();
    }
}