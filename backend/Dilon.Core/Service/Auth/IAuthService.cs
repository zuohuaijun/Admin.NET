using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Dilon.Core.Service
{
    public interface IAuthService
    {
        Task<dynamic> GetCaptcha();
        Task<dynamic> GetCaptchaOpen();
        Task<LoginOutput> GetLoginUserAsync();
        Task<string> LoginAsync([Required] LoginInput input);
        Task LogoutAsync();
        Task<dynamic> VerificationCode(ClickWordCaptchaInput input);
    }
}