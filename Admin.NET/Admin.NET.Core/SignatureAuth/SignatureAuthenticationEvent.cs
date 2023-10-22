// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

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