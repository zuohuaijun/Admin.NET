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
/// 数据操作类型枚举
/// </summary>
[Description("数据操作类型枚举")]
public enum DataOpTypeEnum
{
    /// <summary>
    /// 其它
    /// </summary>
    [Description("其它")]
    Other,

    /// <summary>
    /// 增加
    /// </summary>
    [Description("增加")]
    Add,

    /// <summary>
    /// 删除
    /// </summary>
    [Description("删除")]
    Delete,

    /// <summary>
    /// 编辑
    /// </summary>
    [Description("编辑")]
    Edit,

    /// <summary>
    /// 更新
    /// </summary>
    [Description("更新")]
    Update,

    /// <summary>
    /// 查询
    /// </summary>
    [Description("查询")]
    Query,

    /// <summary>
    /// 详情
    /// </summary>
    [Description("详情")]
    Detail,

    /// <summary>
    /// 树
    /// </summary>
    [Description("树")]
    Tree,

    /// <summary>
    /// 导入
    /// </summary>
    [Description("导入")]
    Import,

    /// <summary>
    /// 导出
    /// </summary>
    [Description("导出")]
    Export,

    /// <summary>
    /// 授权
    /// </summary>
    [Description("授权")]
    Grant,

    /// <summary>
    /// 强退
    /// </summary>
    [Description("强退")]
    Force,

    /// <summary>
    /// 清空
    /// </summary>
    [Description("清空")]
    Clean
}