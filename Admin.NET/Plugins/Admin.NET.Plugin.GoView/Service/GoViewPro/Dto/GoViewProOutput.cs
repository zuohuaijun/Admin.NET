// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

namespace Admin.NET.Plugin.GoView.Service.Dto;

/// <summary>
/// GoView 项目 Item
/// </summary>
public class GoViewProItemOutput
{
    /// <summary>
    /// 项目Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 项目名称
    /// </summary>
    public string ProjectName { get; set; }

    /// <summary>
    /// 项目状态
    /// </summary>
    public GoViewProState State { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime? CreateTime { get; set; }

    /// <summary>
    /// 预览图片url
    /// </summary>
    public string IndexImage { get; set; }

    /// <summary>
    /// 创建者Id
    /// </summary>
    public long? CreateUserId { get; set; }

    /// <summary>
    /// 项目备注
    /// </summary>
    public string Remarks { get; set; }
}

/// <summary>
/// GoView 项目详情
/// </summary>
public class GoViewProDetailOutput : GoViewProItemOutput
{
    /// <summary>
    /// 项目内容
    /// </summary>
    public string Content { get; set; }
}

/// <summary>
/// GoView 新增项目输出
/// </summary>
public class GoViewProCreateOutput
{
    /// <summary>
    /// 项目Id
    /// </summary>
    public long Id { get; set; }
}

/// <summary>
/// GoView 上传项目输出
/// </summary>
public class GoViewProUploadOutput
{
    /// <summary>
    /// Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 仓储名称
    /// </summary>
    public string BucketName { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime? CreateTime { get; set; }

    /// <summary>
    /// 创建者Id
    /// </summary>
    public long? CreateUserId { get; set; }

    /// <summary>
    /// 文件名称
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// 文件大小KB
    /// </summary>
    public int FileSize { get; set; }

    /// <summary>
    /// 文件后缀
    /// </summary>
    public string FileSuffix { get; set; }

    /// <summary>
    /// 文件 Url
    /// </summary>
    [JsonProperty("fileurl")]
    public string FileUrl { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime? UpdateTime { get; set; }

    /// <summary>
    /// 修改者Id
    /// </summary>
    public long? UpdateUserId { get; set; }
}