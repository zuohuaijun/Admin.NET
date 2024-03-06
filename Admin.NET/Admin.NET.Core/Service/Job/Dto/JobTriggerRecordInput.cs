// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core.Service;

public class PageJobTriggerRecordInput : BasePageInput
{
    /// <summary>
    /// 作业Id
    /// </summary>
    public string JobId { get; set; }

    /// <summary>
    /// 触发器Id
    /// </summary>
    public string TriggerId { get; set; }
}