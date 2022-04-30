using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Core.Service
{
    public class PosInput : BaseIdInput
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
        public virtual int Order { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public virtual int Status { get; set; }
    }

    public class AddPosInput : PosInput
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "职位名称不能为空")]
        public override string Name { get; set; }
    }

    public class UpdatePosInput : AddPosInput
    {
    }

    public class DeletePosInput : BaseIdInput
    {
    }
}