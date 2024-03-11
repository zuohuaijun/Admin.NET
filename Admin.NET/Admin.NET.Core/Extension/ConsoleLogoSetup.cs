// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core;

/// <summary>
/// 控制台logo
/// </summary>
public static class ConsoleLogoSetup
{
    public static void AddConsoleLogo(this IServiceCollection services)
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine(@"
              _           _         _   _ ______ _______
     /\      | |         (_)       | \ | |  ____|__   __|
    /  \   __| |_ __ ___  _ _ __   |  \| | |__     | |
   / /\ \ / _` | '_ ` _ \| | '_ \  | . ` |  __|    | |
  / ____ \ (_| | | | | | | | | | |_| |\  | |____   | |
 /_/    \_\__,_|_| |_| |_|_|_| |_(_)_| \_|______|  |_| ");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(@"源码地址: https://gitee.com/zuohuaijun/Admin.NET");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(@"让.NET更简单、更通用、更流行！");
    }
}