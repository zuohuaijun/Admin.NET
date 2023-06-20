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