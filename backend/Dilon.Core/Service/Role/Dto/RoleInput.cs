using System.ComponentModel.DataAnnotations;

namespace Dilon.Core.Service
{
    /// <summary>
    /// 角色参数
    /// </summary>
    public class RoleInput : XnInputBase
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
        /// 数据范围类型（字典 1全部数据 2本部门及以下数据 3本部门数据 4仅本人数据 5自定义数据）
        /// </summary>
        public int DataScopeType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

    public class AddRoleInput : RoleInput
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "角色名称不能为空")]
        public override string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [Required(ErrorMessage = "角色编码不能为空")]
        public override string Code { get; set; }
    }

    public class DeleteRoleInput
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        [Required(ErrorMessage = "角色Id不能为空")]
        public long Id { get; set; }
    }

    public class UpdateRoleInput : AddRoleInput
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        [Required(ErrorMessage = "角色Id不能为空")]
        public long Id { get; set; }
    }

    public class QueryRoleInput : DeleteRoleInput
    {
    }

    public class GrantRoleMenuInput : RoleInput
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        [Required(ErrorMessage = "角色Id不能为空")]
        public long Id { get; set; }
    }

    public class GrantRoleDataInput : GrantRoleMenuInput
    {
    }
}