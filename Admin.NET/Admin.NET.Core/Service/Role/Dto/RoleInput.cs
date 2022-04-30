using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Core.Service
{
    public class RoleInput : BaseIdInput
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
        /// 数据范围类型
        /// </summary>
        public virtual int DataScope { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
    }

    public class PageRoleInput : BasePageInput
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
        /// 数据范围类型
        /// </summary>
        public virtual int DataScope { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public virtual int Status { get; set; }
    }

    public class AddRoleInput : RoleInput
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "角色名称不能为空")]
        public override string Name { get; set; }
    }

    public class UpdateRoleInput : AddRoleInput
    {
    }

    public class DeleteRoleInput : BaseIdInput
    {
    }
}