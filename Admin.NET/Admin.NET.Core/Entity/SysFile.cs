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
/// 系统文件表
/// </summary>
[SugarTable(null, "系统文件表")]
[SysTable]
public class SysFile : EntityBase
{
    /// <summary>
    /// 提供者
    /// </summary>
    [SugarColumn(ColumnDescription = "提供者", Length = 128)]
    [MaxLength(128)]
    public string? Provider { get; set; }

    /// <summary>
    /// 仓储名称
    /// </summary>
    [SugarColumn(ColumnDescription = "仓储名称", Length = 128)]
    [MaxLength(128)]
    public string? BucketName { get; set; }

    /// <summary>
    /// 文件名称（源文件名）
    /// </summary>
    [SugarColumn(ColumnDescription = "文件名称", Length = 128)]
    [MaxLength(128)]
    public string? FileName { get; set; }

    /// <summary>
    /// 文件后缀
    /// </summary>
    [SugarColumn(ColumnDescription = "文件后缀", Length = 16)]
    [MaxLength(16)]
    public string? Suffix { get; set; }

    /// <summary>
    /// 存储路径
    /// </summary>
    [SugarColumn(ColumnDescription = "存储路径", Length = 128)]
    [MaxLength(128)]
    public string? FilePath { get; set; }

    /// <summary>
    /// 文件大小KB
    /// </summary>
    [SugarColumn(ColumnDescription = "文件大小KB", Length = 16)]
    [MaxLength(16)]
    public string? SizeKb { get; set; }

    /// <summary>
    /// 文件大小信息-计算后的
    /// </summary>
    [SugarColumn(ColumnDescription = "文件大小信息", Length = 64)]
    [MaxLength(64)]
    public string? SizeInfo { get; set; }

    /// <summary>
    /// 外链地址-OSS上传后生成外链地址方便前端预览
    /// </summary>
    [SugarColumn(ColumnDescription = "外链地址", Length = 512)]
    [MaxLength(512)]
    public string? Url { get; set; }

    /// <summary>
    /// 文件MD5
    /// </summary>
    [SugarColumn(ColumnDescription = "文件MD5", Length = 128)]
    [MaxLength(128)]
    public string? FileMd5 { get; set; }
}