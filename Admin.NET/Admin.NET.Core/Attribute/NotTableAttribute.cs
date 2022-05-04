using Furion.DependencyInjection;
using System;

namespace Admin.NET.Core
{
    /// <summary>
    /// 非实体表特性
    /// </summary>
    [SuppressSniffer, AttributeUsage(AttributeTargets.Class)]
    public class NotTableAttribute : Attribute
    {
    }
}