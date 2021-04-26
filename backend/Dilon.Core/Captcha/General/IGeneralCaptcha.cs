namespace Dilon.Core
{
    public interface IGeneralCaptcha
    {
        dynamic CheckCode(GeneralCaptchaInput input);

        string CreateCaptchaImage(int length = 4);
    }
}