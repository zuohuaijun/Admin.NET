using System.ComponentModel.DataAnnotations;

namespace Dilon.Core.Service
{
    /// <summary>
    /// 租户参数
    /// </summary>
    public class TenantInput : PageInputBase
    {
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 主机
        /// </summary>
        public virtual string Host { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 模式
        /// </summary>
        public string Schema { get; set; }

        /// <summary>
        /// 数据库连接
        /// </summary>
        public virtual string Connection { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreatedTime { get; set; }
    }

    public class AddTenantInput : TenantInput
    {
        /// <summary>
        /// 租户名称
        /// </summary>
        [Required(ErrorMessage = "租户名称不能为空")]
        public override string Name { get; set; }

        /// <summary>
        /// 主机名称
        /// </summary>
        [Required(ErrorMessage = "主机名称不能为空")]
        public override string Host { get; set; }

        /// <summary>
        /// 数据库连接
        /// </summary>
        [Required(ErrorMessage = "数据库连接不能为空")]
        public override string Connection { get; set; }
    }

    public class DeleteTenantInput
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        [Required(ErrorMessage = "租户Id不能为空")]
        public long Id { get; set; }
    }

    public class UpdateTenantInput : TenantInput
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        [Required(ErrorMessage = "租户Id不能为空")]
        public long Id { get; set; }
    }

    public class QueryTenantInput : DeleteTenantInput
    {

    }
}
