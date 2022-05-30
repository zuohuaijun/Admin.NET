using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Core.Service
{
    public class DistrictInput : BaseIdInput
    {
        /// <summary>
        /// 父Id
        /// </summary>
        public virtual long Pid { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 值 
        ///</summary>
        public virtual string Value { get; set; }

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

    public class AddDistrictInput : DistrictInput
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "资源名称不能为空")]
        public override string Name { get; set; }
    }

    public class UpdateDistrictInput : AddDistrictInput
    {
    }

    public class DeleteDistrictInput : BaseIdInput
    {
    }
}
