// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core.Service;

public class PageNoticeInput : BasePageInput
{
    /// <summary>
    /// 标题
    /// </summary>
    public virtual string Title { get; set; }

    /// <summary>
    /// 类型（1通知 2公告）
    /// </summary>
    public virtual NoticeTypeEnum? Type { get; set; }
}

public class AddNoticeInput : SysNotice
{
}

public class UpdateNoticeInput : AddNoticeInput
{
}

public class DeleteNoticeInput : BaseIdInput
{
}

public class NoticeInput : BaseIdInput
{
}