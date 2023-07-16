// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

namespace Admin.NET.Core.Service;

/// <summary>
/// 代码生成参数类
/// </summary>
public class CodeGenOutput
{
    /// <summary>
    /// 代码生成器Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 作者姓名
    /// </summary>
    public string AuthorName { get; set; }

    /// <summary>
    /// 类名
    /// </summary>
    public string ClassName { get; set; }

    /// <summary>
    /// 是否移除表前缀
    /// </summary>
    public string TablePrefix { get; set; }

    /// <summary>
    /// 生成方式
    /// </summary>
    public string GenerateType { get; set; }

    /// <summary>
    /// 数据库表名
    /// </summary>
    public string TableName { get; set; }

    /// <summary>
    /// 包名
    /// </summary>
    public string PackageName { get; set; }

    /// <summary>
    /// 业务名（业务代码包名称）
    /// </summary>
    public string BusName { get; set; }

    /// <summary>
    /// 功能名（数据库表名称）
    /// </summary>
    public string TableComment { get; set; }

    /// <summary>
    /// 菜单应用分类（应用编码）
    /// </summary>
    public string MenuApplication { get; set; }

    /// <summary>
    /// 菜单父级
    /// </summary>
    public long MenuPid { get; set; }
}