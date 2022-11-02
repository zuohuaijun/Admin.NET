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
    public virtual NoticeTypeEnum Type { get; set; }
}

[NotTable]
public class AddNoticeInput : SysNotice
{
}

[NotTable]
public class UpdateNoticeInput : AddNoticeInput
{
}

public class DeleteNoticeInput : BaseIdInput
{
}

public class NoticeInput : BaseIdInput
{
}