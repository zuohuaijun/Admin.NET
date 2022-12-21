namespace Admin.NET.Core;

/// <summary>
/// 系统表特性
/// </summary>
[SuppressSniffer]
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class SystemTableAttribute : Attribute
{
}