namespace Admin.NET.Core;

/// <summary>
/// 租户业务表特性
/// </summary>
[SuppressSniffer]
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class TenantBusinessAttribute : Attribute
{
}