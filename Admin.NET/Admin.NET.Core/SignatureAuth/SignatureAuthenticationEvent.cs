// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core;

/// <summary>
/// Signature 身份验证事件
/// </summary>
public class SignatureAuthenticationEvent
{
    public SignatureAuthenticationEvent()
    {
    }

    /// <summary>
    /// 获取或设置获取 AccessKey 的 AccessSecret 的逻辑处理
    /// </summary>
    public Func<GetAccessSecretContext, Task<string>> OnGetAccessSecret { get; set; }

    /// <summary>
    /// 获取或设置质询的逻辑处理
    /// </summary>
    public Func<SignatureChallengeContext, Task> OnChallenge { get; set; } = _ => Task.CompletedTask;

    /// <summary>
    /// 获取或设置已验证的逻辑处理
    /// </summary>
    public Func<SignatureValidatedContext, Task> OnValidated { get; set; } = _ => Task.CompletedTask;

    /// <summary>
    /// 获取 AccessKey 的 AccessSecret
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public virtual Task<string> GetAccessSecret(GetAccessSecretContext context) => OnGetAccessSecret?.Invoke(context) ?? throw new NotImplementedException($"需要提供 {nameof(OnGetAccessSecret)} 实现");

    /// <summary>
    /// 质询
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public virtual Task Challenge(SignatureChallengeContext context) => OnChallenge?.Invoke(context) ?? Task.CompletedTask;

    /// <summary>
    /// 已验证成功
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public virtual Task Validated(SignatureValidatedContext context) => OnValidated?.Invoke(context) ?? Task.CompletedTask;
}