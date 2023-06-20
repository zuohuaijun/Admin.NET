namespace Admin.NET.Plugin.GoView.Service.Dto;

/// <summary>
/// GoView 新增项目
/// </summary>
public class ProjectCreateInput
{
    /// <summary>
    /// 项目名称
    /// </summary>
    public string ProjectName { get; set; }

    /// <summary>
    /// 项目备注
    /// </summary>
    public string Remarks { get; set; }

    /// <summary>
    /// 预览图片url
    /// </summary>
    public string IndexImage { get; set; }
}

/// <summary>
/// GoView 保存项目
/// </summary>
public class ProjectSaveDataInput
{
    /// <summary>
    /// 项目Id
    /// </summary>
    public long ProjectId { get; set; }

    /// <summary>
    /// 项目参数
    /// </summary>
    public string Content { get; set; }
}

/// <summary>
/// GoView 编辑项目
/// </summary>
public class ProjectEditInput
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
    /// 预览图片url
    /// </summary>
    public string IndexImage { get; set; }
}

/// <summary>
/// GoView 修改项目发布状态
/// </summary>
public class ProjectPublishInput
{
    /// <summary>
    /// 项目Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 项目状态
    /// </summary>
    public GoViewProjectState State { get; set; }
}