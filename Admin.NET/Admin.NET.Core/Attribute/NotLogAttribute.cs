namespace Admin.NET.Core;

/// <summary>
/// 禁用日志特性
/// </summary>
[SuppressSniffer, AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
public class NotLogAttribute : Attribute
{
}