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