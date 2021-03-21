using System.ComponentModel.DataAnnotations;

namespace Dilon.Core.Service
{
    /// <summary>
    /// 参数配置
    /// </summary>
    public class ConfigInput : PageInputBase
    {
        /// <summary>
        /// 参数Id
        /// </summary>
        public virtual long Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// 属性值
        /// </summary>
        public virtual string Value { get; set; }

        /// <summary>
        /// 是否是系统参数（Y-是，N-否）
        /// </summary>
        public virtual string SysFlag { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }

        /// <summary>
        /// 状态（字典 0正常 1停用 2删除）
        /// </summary>
        public virtual int Status { get; set; }

        /// <summary>
        /// 常量所属分类的编码，来自于“常量的分类”字典
        /// </summary>
        public virtual string GroupCode { get; set; }
    }

    public class AddConfigInput : ConfigInput
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "参数名称不能为空")]
        public override string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [Required(ErrorMessage = "参数编码不能为空")]
        public override string Code { get; set; }
    }

    public class DeleteConfigInput
    {
        /// <summary>
        /// 应用Id
        /// </summary>
        [Required(ErrorMessage = "参数Id不能为空")]
        public long Id { get; set; }
    }

    public class UpdateConfigInput : AddConfigInput
    {
        /// <summary>
        /// 应用Id
        /// </summary>
        [Required(ErrorMessage = "应用Id不能为空")]
        public override long Id { get; set; }
    }

    public class QueryConfigInput : DeleteConfigInput
    {

    }
}
