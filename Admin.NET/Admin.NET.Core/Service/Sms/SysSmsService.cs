// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using AlibabaCloud.SDK.Dysmsapi20170525.Models;

namespace Admin.NET.Core.Service;

/// <summary>
/// 系统短信服务
/// </summary>
[AllowAnonymous]
[ApiDescriptionSettings(Order = 150)]
public class SysSmsService : IDynamicApiController, ITransient
{
    private readonly SMSOptions _smsOptions;
    private readonly SysCacheService _sysCacheService;

    public SysSmsService(IOptions<SMSOptions> smsOptions,
        SysCacheService sysCacheService)
    {
        _smsOptions = smsOptions.Value;
        _sysCacheService = sysCacheService;
    }

    /// <summary>
    /// 发送短信
    /// </summary>
    /// <param name="phoneNumber"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [DisplayName("发送短信")]
    public async Task SendSms([Required] string phoneNumber)
    {
        if (!phoneNumber.TryValidate(ValidationTypes.PhoneNumber).IsValid)
            throw Oops.Oh("请正确填写手机号码");

        // 生成随机验证码
        var random = new Random();
        var verifyCode = random.Next(100000, 999999);

        var templateParam = Clay.Object(new
        {
            code = verifyCode
        });

        try
        {
            var client = CreateClient();
            var sendSmsRequest = new SendSmsRequest
            {
                PhoneNumbers = phoneNumber, // 待发送手机号, 多个以逗号分隔
                SignName = _smsOptions.Aliyun.SignName, // 短信签名
                TemplateCode = _smsOptions.Aliyun.TemplateCode, // 短信模板
                TemplateParam = templateParam.ToString(), // 模板中的变量替换JSON串
                OutId = YitIdHelper.NextId().ToString()
            };
            var sendSmsResponse = client.SendSms(sendSmsRequest);
            if (sendSmsResponse.Body.Code == "OK" && sendSmsResponse.Body.Message == "OK")
            {
                // var bizId = sendSmsResponse.Body.BizId;
                _sysCacheService.Set($"{CacheConst.KeyPhoneVerCode}{phoneNumber}", verifyCode, TimeSpan.FromSeconds(60));
            }
            else
            {
                throw Oops.Oh($"短信发送失败：{sendSmsResponse.Body.Code}-{sendSmsResponse.Body.Message}");
            }
        }
        catch (Exception ex)
        {
            throw Oops.Oh($"短信发送失败：{ex.Message}");
        }

        await Task.CompletedTask;
    }

    /// <summary>
    /// 阿里云短信配置
    /// </summary>
    /// <returns></returns>
    private AlibabaCloud.SDK.Dysmsapi20170525.Client CreateClient()
    {
        var config = new AlibabaCloud.OpenApiClient.Models.Config
        {
            AccessKeyId = _smsOptions.Aliyun.AccessKeyId,
            AccessKeySecret = _smsOptions.Aliyun.AccessKeySecret,
            Endpoint = "dysmsapi.aliyuncs.com"
        };
        return new AlibabaCloud.SDK.Dysmsapi20170525.Client(config);
    }
}