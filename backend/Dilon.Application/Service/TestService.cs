using Furion.DependencyInjection;
using Furion.DynamicApiController;

namespace Dilon.Application
{
    /// <summary>
    /// 事例
    /// </summary>
    public class TestService : ITestService, IDynamicApiController, ITransient
    {
        public TestService()
        {

        }

        public string GetDescription()
        {
            return "Admin.NET";
        }
    }
}
