// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core.Service;

public class JobDetailOutput
{
    /// <summary>
    /// 作业信息
    /// </summary>
    public SysJobDetail JobDetail { get; set; }

    /// <summary>
    /// 触发器集合
    /// </summary>
    public List<SysJobTrigger> JobTriggers { get; set; }
}