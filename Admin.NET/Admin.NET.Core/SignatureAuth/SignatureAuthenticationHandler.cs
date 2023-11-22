// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Encodings.Web;

namespace Admin.NET.Core;

/// <summary>
/// Signature 身份验证处理
/// </summary>
public sealed class SignatureAuthenticationHandler : AuthenticationHandler<SignatureAuthenticationOptions>
{
    private readonly SysCacheService _cacheService;

    public SignatureAuthenticationHandler(IOptionsMonitor<SignatureAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        SysCacheService cacheService)
        : base(options, logger, encoder, clock)
    {
        _cacheService = cacheService;
    }

    private new SignatureAuthenticationEvent Events
    {
        get => (SignatureAuthenticationEvent)base.Events;
        set => base.Events = value;
    }

    /// <summary>
    /// 确保创建的 Event 类型是 DigestEvents
    /// </summary>
    /// <returns></returns>
    protected override Task<object> CreateEventsAsync() => throw new NotImplementedException($"{nameof(SignatureAuthenticationOptions)}.{nameof(SignatureAuthenticationOptions.Events)} 需要提供一个实例");

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var accessKey = Request.Headers["accessKey"].FirstOrDefault();
        var timestampStr = Request.Headers["timestamp"].FirstOrDefault(); // 精确到秒
        var nonce = Request.Headers["nonce"].FirstOrDefault();
        var sign = Request.Headers["sign"].FirstOrDefault();

        if (string.IsNullOrEmpty(accessKey))
            return await AuthenticateResultFailAsync("accessKey 不能为空");
        if (string.IsNullOrEmpty(timestampStr))
            return await AuthenticateResultFailAsync("timestamp 不能为空");
        if (string.IsNullOrEmpty(nonce))
            return await AuthenticateResultFailAsync("nonce 不能为空");
        if (string.IsNullOrEmpty(sign))
            return await AuthenticateResultFailAsync("sign 不能为空");

        // 验证请求数据是否在可接受的时间内
        if (!long.TryParse(timestampStr, out var timestamp))
            return await AuthenticateResultFailAsync("timestamp 值不合法");

        var requestDate = DateTimeUtil.ToLocalTimeDateBySeconds(timestamp);
        if (requestDate > Clock.UtcNow.Add(Options.AllowedDateDrift).LocalDateTime || requestDate < Clock.UtcNow.Subtract(Options.AllowedDateDrift).LocalDateTime)
            return await AuthenticateResultFailAsync("timestamp 值已超过允许的偏差范围");

        // 获取 accessSecret
        var getAccessSecretContext = new GetAccessSecretContext(Context, Scheme, Options) { AccessKey = accessKey };
        var accessSecret = await Events.GetAccessSecret(getAccessSecretContext);
        if (string.IsNullOrEmpty(accessSecret))
            return await AuthenticateResultFailAsync("accessKey 无效");

        // 校验签名
        var appSecretByte = Encoding.UTF8.GetBytes(accessSecret);
        string serverSign = SignData(appSecretByte, GetMessageForSign(Context));

        if (serverSign != sign)
            return await AuthenticateResultFailAsync("sign 无效的签名");

        // 重放检测
        var cacheKey = $"{CacheConst.KeyOpenAccessNonce}{accessKey}|{nonce}";
        if (_cacheService.ExistKey(cacheKey))
            return await AuthenticateResultFailAsync("重复的请求");
        _cacheService.Set(cacheKey, null, Options.AllowedDateDrift * 2); // 缓存过期时间为偏差范围时间的2倍

        // 已验证成功
        var signatureValidatedContext = new SignatureValidatedContext(Context, Scheme, Options)
        {
            Principal = new ClaimsPrincipal(new ClaimsIdentity(SignatureAuthenticationDefaults.AuthenticationScheme)),
            AccessKey = accessKey
        };
        await Events.Validated(signatureValidatedContext);
        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        if (signatureValidatedContext.Result != null)
            return signatureValidatedContext.Result;

        // ReSharper disable once HeuristicUnreachableCode
        signatureValidatedContext.Success();
        return signatureValidatedContext.Result;
    }

    protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        var authResult = await HandleAuthenticateOnceSafeAsync();
        var challengeContext = new SignatureChallengeContext(Context, Scheme, Options, properties)
        {
            AuthenticateFailure = authResult.Failure,
        };
        await Events.Challenge(challengeContext);
        // 质询已处理
        if (challengeContext.Handled) return;

        await base.HandleChallengeAsync(properties);
    }

    /// <summary>
    /// 获取用于签名的消息
    /// </summary>
    /// <returns></returns>
    private static string GetMessageForSign(HttpContext context)
    {
        var method = context.Request.Method; // 请求方法（大写）
        var url = context.Request.Path; // 请求 url，去除协议、域名、参数，以 / 开头
        var accessKey = context.Request.Headers["accessKey"].FirstOrDefault(); // 身份标识
        var timestamp = context.Request.Headers["timestamp"].FirstOrDefault(); // 时间戳，精确到秒
        var nonce = context.Request.Headers["nonce"].FirstOrDefault(); // 唯一随机数

        return $"{method}&{url}&{accessKey}&{timestamp}&{nonce}";
    }

    /// <summary>
    /// 对数据进行签名
    /// </summary>
    /// <param name="secret"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    private static string SignData(byte[] secret, string data)
    {
        if (secret == null)
            throw new ArgumentNullException(nameof(secret));

        if (data == null)
            throw new ArgumentNullException(nameof(data));

        using HMAC hmac = new HMACSHA256();
        hmac.Key = secret;
        return Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(data)));
    }

    /// <summary>
    /// 返回验证失败结果，并在 Items 中增加 <see cref="SignatureAuthenticationDefaults.AuthenticateFailMsgKey"/>，记录身份验证失败消息
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    private Task<AuthenticateResult> AuthenticateResultFailAsync(string message)
    {
        // 写入身份验证失败消息
        Context.Items[SignatureAuthenticationDefaults.AuthenticateFailMsgKey] = message;
        return Task.FromResult(AuthenticateResult.Fail(message));
    }
}