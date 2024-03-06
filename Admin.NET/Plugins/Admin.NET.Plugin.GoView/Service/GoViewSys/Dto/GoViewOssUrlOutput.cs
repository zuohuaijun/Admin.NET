// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

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