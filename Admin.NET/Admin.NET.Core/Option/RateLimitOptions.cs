// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

using AspNetCoreRateLimit;

namespace Admin.NET.Core;

/// <summary>
/// IP限流配置选项
/// </summary>
public sealed class IpRateLimitingOptions : IpRateLimitOptions
{
}

/// <summary>
/// IP限流策略配置选项
/// </summary>
public sealed class IpRateLimitPoliciesOptions : IpRateLimitPolicies, IConfigurableOptions
{
}

/// <summary>
/// 客户端限流配置选项
/// </summary>
public sealed class ClientRateLimitingOptions : ClientRateLimitOptions, IConfigurableOptions
{
}

/// <summary>
/// 客户端限流策略配置选项
/// </summary>
public sealed class ClientRateLimitPoliciesOptions : ClientRateLimitPolicies, IConfigurableOptions
{
}