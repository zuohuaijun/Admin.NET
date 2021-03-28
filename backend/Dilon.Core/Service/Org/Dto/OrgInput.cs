using System.ComponentModel.DataAnnotations;

namespace Dilon.Core.Service
{
    /// <summary>
    /// 组织机构参数
    /// </summary>
    public class OrgInput : XnInputBase
    {
        /// <summary>
        /// 父Id
        /// </summary>
        public string Pid { get; set; }

        /// <summary>
        /// 父Ids
        /// </summary>
        public string Pids { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public virtual string Tel { get; set; }

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

    public class AddOrgInput : OrgInput
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "机构名称不能为空")]
        public override string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [Required(ErrorMessage = "机构编码不能为空")]
        public override string Code { get; set; }
    }

    public class DeleteOrgInput
    {
        /// <summary>
        /// 机构Id
        /// </summary>
        [Required(ErrorMessage = "机构Id不能为空")]
        public string Id { get; set; }
    }

    public class UpdateOrgInput : AddOrgInput
    {
        /// <summary>
        /// 机构Id
        /// </summary>
        [Required(ErrorMessage = "机构Id不能为空")]
        public string Id { get; set; }
    }

    public class QueryOrgInput : DeleteOrgInput
    {

    }

    public class PageOrgInput : OrgInput
    {
        public string Id { get; set; }
    }
}
