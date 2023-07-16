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
/// 正则校验
/// </summary>
public static class RegularValidate
{
    /// <summary>
    /// 验证密码规则
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    public static bool ValidatePassword(string password)
    {
        var regex = new Regex(@"
(?=.*[0-9])                     #必须包含数字
(?=.*[a-z])                     #必须包含小写
(?=.*[A-Z])                     #必须包含大写
(?=([\x21-\x7e]+)[^a-zA-Z0-9])  #必须包含特殊符号
.{8,30}                         #至少8个字符，最多30个字符
", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);

        //如果要求必须包含小写、大写字母，则上面的(?=.*[a-zA-Z]) 要改为：
        /*
         * (?=.*[a-z])
         * (?=.*[A-Z])
         */
        return regex.IsMatch(password);
    }
}