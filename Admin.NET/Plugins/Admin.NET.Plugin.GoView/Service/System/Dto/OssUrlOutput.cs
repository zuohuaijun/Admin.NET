namespace Admin.NET.Plugin.GoView.Service.Dto;

/// <summary>
/// 获取 oss 上传接口输出
/// </summary>
public class OssUrlOutput
{
    /// <summary>
    /// bucket 地址
    /// </summary>
    [JsonProperty("bucketURL")]
    public string BucketUrl { get; set; }
}