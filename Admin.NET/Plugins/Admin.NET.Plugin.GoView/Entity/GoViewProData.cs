// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Plugin.GoView.Entity;

/// <summary>
/// GoView 项目数据表
/// </summary>
[SugarTable(null, "GoView 项目数据表")]
public class GoViewProData : EntityTenant
{
    /// <summary>
    /// 项目内容
    /// </summary>
    [SugarColumn(ColumnDescription = "项目内容", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public string? Content { get; set; }

    /// <summary>
    /// 预览图片
    /// </summary>
    [SugarColumn(ColumnDescription = "预览图片", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public string? IndexImageData { get; set; }
}