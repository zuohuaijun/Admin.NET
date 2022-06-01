namespace Admin.NET.Core.Service;

public class FileInput : BaseIdInput
{
}

public class PageFileInput : BasePageInput
{
    ///// <summary>
    ///// 仓储名称
    ///// </summary>
    //public OSSProvider BucketName { get; set; }

    /// <summary>
    /// 原始名称
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? EndTime { get; set; }
}

public class DeleteFileInput : BaseIdInput
{
}