using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Core.Service
{
    /// <summary>
    /// 租户参数
    /// </summary>
    public class TenantPageInput : PageInputBase
    {
        /// <summary>
        /// 公司名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 主机
        /// </summary>
        public virtual string Host { get; set; }
    }

    public class AddTenantInput
    {
        /// <summary>
        /// 公司名称
        /// </summary>
        [Required(ErrorMessage = "公司名称")]
        public string Name { get; set; }

        /// <summary>
        /// 管理员名称
        /// </summary>
        [Required(ErrorMessage = "管理员名称")]
        public string AdminName { get; set; }

        /// <summary>
        /// 主机名称
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 数据库连接
        /// </summary>
        public string Connection { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        [Required(ErrorMessage = "电子邮箱")]
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
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreatedTime { get; set; }
    }

    public class DeleteTenantInput : BaseId
    {
    }

    public class UpdateTenantInput : BaseId
    {
        /// <summary>
        /// 公司名称
        /// </summary>
        [Required(ErrorMessage = "公司名称")]
        public string Name { get; set; }

        /// <summary>
        /// 管理员名称
        /// </summary>
        [Required(ErrorMessage = "管理员名称")]
        public string AdminName { get; set; }

        /// <summary>
        /// 主机名称
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 数据库连接
        /// </summary>
        public string Connection { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        [Required(ErrorMessage = "电子邮箱")]
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
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreatedTime { get; set; }
    }

    public class QueryTenantInput : BaseId
    {
    }
}