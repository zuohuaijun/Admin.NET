using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Core.Service
{
    /// <summary>
    /// 通知公告参数
    /// </summary>
    public class NoticePageInput : PageInputBase
    {
        /// <summary>
        /// 类型（字典 1通知 2公告）
        /// </summary>
        public virtual int Type { get; set; }
    }

    public class AddNoticeInput
    {
        /// <summary>
        /// 标题
        /// </summary>
        [Required(ErrorMessage = "标题不能为空")]
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [Required(ErrorMessage = "内容不能为空")]
        public string Content { get; set; }

        /// <summary>
        /// 类型（字典 1通知 2公告）
        /// </summary>
        [Required(ErrorMessage = "类型不能为空")]
        public int Type { get; set; }

        /// <summary>
        /// 状态（字典 0草稿 1发布 2撤回 3删除）
        /// </summary>
        [Required(ErrorMessage = "状态不能为空")]
        public NoticeStatus Status { get; set; }

        /// <summary>
        /// 通知到的人
        /// </summary>
        [Required(ErrorMessage = "通知到的人不能为空")]
        public List<long> NoticeUserIdList { get; set; }
    }

    public class DeleteNoticeInput : BaseId
    {
    }

    public class UpdateNoticeInput
    {
        /// <summary>
        /// Id
        /// </summary>
        [Required(ErrorMessage = "通知公告Id不能为空")]
        public long Id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Required(ErrorMessage = "标题不能为空")]
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [Required(ErrorMessage = "内容不能为空")]
        public string Content { get; set; }

        /// <summary>
        /// 类型（字典 1通知 2公告）
        /// </summary>
        [Required(ErrorMessage = "类型不能为空")]
        public int Type { get; set; }

        /// <summary>
        /// 状态（字典 0草稿 1发布 2撤回 3删除）
        /// </summary>
        [Required(ErrorMessage = "状态不能为空")]
        public NoticeStatus Status { get; set; }

        /// <summary>
        /// 通知到的人
        /// </summary>
        [Required(ErrorMessage = "通知到的人不能为空")]
        public List<long> NoticeUserIdList { get; set; }
    }

    public class QueryNoticeInput : BaseId
    {
    }

    public class ChangeStatusNoticeInput : BaseId
    {
        /// <summary>
        /// 状态（字典 0草稿 1发布 2撤回 3删除）
        /// </summary>
        [Required(ErrorMessage = "状态不能为空")]
        public NoticeStatus Status { get; set; }
    }
}