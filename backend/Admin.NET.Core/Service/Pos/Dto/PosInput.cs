using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Core.Service
{
    /// <summary>
    /// 职位参数
    /// </summary>
    public class PosInput
    {
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 状态（字典 0正常 1停用 2删除）
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageNo { get; set; } = 1;

        /// <summary>
        /// 页码容量
        /// </summary>
        public int PageSize { get; set; } = 20;
    }

    public class AddPosInput
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "职位名称不能为空")]
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [Required(ErrorMessage = "职位编码不能为空")]
        public string Code { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 状态（字典 0正常 1停用 2删除）
        /// </summary>
        public int Status { get; set; }
    }

    public class DeletePosInput : BaseId
    {
    }

    public class UpdatePosInput
    {
        /// <summary>
        /// 职位Id
        /// </summary>
        [Required(ErrorMessage = "职位Id不能为空")]
        public long Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "职位名称不能为空")]
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [Required(ErrorMessage = "职位编码不能为空")]
        public string Code { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 状态（字典 0正常 1停用 2删除）
        /// </summary>
        public int Status { get; set; }
    }

    public class QueryPosInput : BaseId
    {
    }
}