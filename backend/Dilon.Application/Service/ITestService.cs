using System.Threading.Tasks;

namespace Dilon.Application
{
    public interface ITestService
    {
        string GetDescription();

        /// <summary>
        /// 发送消息到客户端
        /// </summary>
        Task SendMessage();
    }
}