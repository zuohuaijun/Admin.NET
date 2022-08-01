namespace Admin.NET.Core.Service;

/// <summary>
/// 通知公告参数
/// </summary>
public class NoticeInput : BasePageInput
{
    /// <summary>
    /// 标题
    /// </summary>
    public virtual string Title { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    public virtual string Content { get; set; }

    /// <summary>
    /// 类型（字典 1通知 2公告）
    /// </summary>
    public virtual NoticeTypeEnum Type { get; set; }

    /// <summary>
    /// 状态（字典 0草稿 1发布 2撤回 3删除）
    /// </summary>
    public virtual NoticeStatusEnum Status { get; set; }

    /// <summary>
    /// 通知到的人
    /// </summary>
    public virtual List<long> NoticeUserIdList { get; set; }
}

public class AddNoticeInput : NoticeInput
{
    /// <summary>
    /// 标题
    /// </summary>
    [Required(ErrorMessage = "标题不能为空")]
    public override string Title { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    [Required(ErrorMessage = "内容不能为空")]
    public override string Content { get; set; }

    /// <summary>
    /// 类型（字典 1通知 2公告）
    /// </summary>
    [Required(ErrorMessage = "类型不能为空")]
    public override NoticeTypeEnum Type { get; set; }

    /// <summary>
    /// 状态（字典 0草稿 1发布 2撤回 3删除）
    /// </summary>
    [Required(ErrorMessage = "状态不能为空")]
    public override NoticeStatusEnum Status { get; set; }

    /// <summary>
    /// 通知到的人
    /// </summary>
    [Required(ErrorMessage = "通知到的人不能为空")]
    public override List<long> NoticeUserIdList { get; set; }
}

public class DeleteNoticeInput
{
    /// <summary>
    /// Id
    /// </summary>
    [Required(ErrorMessage = "通知公告Id不能为空")]
    public long Id { get; set; }
}

public class UpdateNoticeInput : AddNoticeInput
{
    /// <summary>
    /// Id
    /// </summary>
    [Required(ErrorMessage = "通知公告Id不能为空")]
    public long Id { get; set; }
}

public class QueryNoticeInput : DeleteNoticeInput
{
}

public class ChangeStatusNoticeInput : DeleteNoticeInput
{
    /// <summary>
    /// 状态（字典 0草稿 1发布 2撤回 3删除）
    /// </summary>
    [Required(ErrorMessage = "状态不能为空")]
    public NoticeStatusEnum Status { get; set; }
}