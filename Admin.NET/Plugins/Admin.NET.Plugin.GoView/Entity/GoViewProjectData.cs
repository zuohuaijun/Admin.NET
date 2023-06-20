namespace Admin.NET.Plugin.GoView.Entity;

/// <summary>
/// GoView 项目数据表
/// </summary>
[SugarTable(null, "GoView 项目数据表")]
public class GoViewProjectData : EntityTenant
{
    /// <summary>
    /// 项目参数
    /// </summary>
    [SugarColumn(ColumnDescription = "项目参数", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public string? Content { get; set; }

    /// <summary>
    /// 预览图片
    /// </summary>
    [SugarColumn(ColumnDescription = "预览图片", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public string? IndexImageData { get; set; }
}