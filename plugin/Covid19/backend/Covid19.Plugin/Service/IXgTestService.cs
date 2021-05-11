using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Covid19.Plugin
{
    public interface IXgTestService
    {
        Task CheckTestResult(IdsInput input);
        Task<dynamic> QueryXgTestPageList([FromQuery] XgTestInput input);
        Task UpdateNegative(IdsInput input);
        Task UpdateTestResult(XgTestInput input);
    }
}