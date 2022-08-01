namespace Admin.NET.Core.Service;

/// <summary>
/// 系统通知公告详情参数
/// </summary>
public class NoticeDetailOutput : NoticeBase
{
    /// <summary>
    /// 通知到的用户Id集合
    /// </summary>
    public List<string> NoticeUserIdList { get; set; }

    /// <summary>
    /// 通知到的用户阅读信息集合
    /// </summary>
    public List<NoticeUserRead> NoticeUserReadInfoList { get; set; }
}

public class NoticeUserRead
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// 用户名称
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 状态（字典 0未读 1已读）
    /// </summary>
    public NoticeUserStatusEnum ReadStatus { get; set; }

    /// <summary>
    /// 阅读时间
    /// </summary>
    public DateTime ReadTime { get; set; }
}