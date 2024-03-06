// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core;

/// <summary>
/// 最大值校验
/// </summary>
public class MaxValueAttribute : ValidationAttribute
{
    private double MaxValue { get; }

    /// <summary>
    /// 最大值
    /// </summary>
    /// <param name="value"></param>
    public MaxValueAttribute(double value) => this.MaxValue = value;

    /// <summary>
    /// 最大值校验
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public override bool IsValid(object value)
    {
        return value == null || Convert.ToDouble(value) <= this.MaxValue;
    }

    /// <summary>
    /// 错误信息
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public override string FormatErrorMessage(string name) => base.FormatErrorMessage(name);
}