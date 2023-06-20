namespace Admin.NET.Plugin.GoView.Service.Dto;

/// <summary>
/// 获取 OSS 上传接口输出
/// </summary>
public class GoViewOssUrlOutput
{
    /// <summary>
    /// 桶名
    /// </summary>
    public string BucketName { get; set; }

    /// <summary>
    /// BucketURL 地址
    /// </summary>
    public string BucketURL { get; set; }
}