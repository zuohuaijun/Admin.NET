using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Core.Service
{
    /// <summary>
    /// 租户管理输入参数
    /// </summary>
    public class TenantInput : BasePageInput
    {
        /// <summary>
        /// 公司名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 管理员名称
        /// </summary>
        public virtual string AdminName { get; set; }

        /// <summary>
        /// 主机
        /// </summary>
        public virtual string Host { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public virtual string Phone { get; set; }

        /// <summary>
        /// 数据库连接
        /// </summary>
        public virtual string Connection { get; set; }

        /// <summary>
        /// 架构
        /// </summary>
        public virtual string Schema { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
    }

    public class AddTenantInput : TenantInput
    {
        /// <summary>
        /// 公司名称
        /// </summary>
        [Required(ErrorMessage = "公司名称不能为空"), MinLength(2, ErrorMessage = "公司名称不能少于2个字符")]
        public override string Name { get; set; }

        /// <summary>
        /// 管理员名称
        /// </summary>
        [Required(ErrorMessage = "管理员名称不能为空"), MinLength(3, ErrorMessage = "管理员名称不能少于3个字符")]
        public override string AdminName { get; set; }
    }

    public class DeleteTenantInput
    {
        /// <summary>
        /// Id
        /// </summary>
        [Required(ErrorMessage = "Id不能为空")]
        public long Id { get; set; }
    }

    public class UpdateTenantInput : TenantInput
    {
        /// <summary>
        /// Id
        /// </summary>
        [Required(ErrorMessage = "Id不能为空")]
        public long Id { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        [Required(ErrorMessage = "公司名称不能为空"), MinLength(2, ErrorMessage = "公司名称不能少于2个字符")]
        public override string Name { get; set; }

        /// <summary>
        /// 管理员名称
        /// </summary>
        [Required(ErrorMessage = "管理员名称不能为空"), MinLength(3, ErrorMessage = "管理员名称不能少于3个字符")]
        public override string AdminName { get; set; }
    }

    public class QueryeTenantInput : DeleteTenantInput
    {
    }
}