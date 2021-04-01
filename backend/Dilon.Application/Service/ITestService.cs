using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dilon.Application
{
    public interface ITestService
    {
        Task InitData();
        Task<List<Test>> GetList();
        string GetDescription();
    }
}