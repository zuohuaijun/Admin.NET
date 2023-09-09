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