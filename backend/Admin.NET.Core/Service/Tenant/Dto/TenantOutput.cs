using System;

namespace Admin.NET.Core.Service
{
    /// <summary>
    /// 租户参数
    /// </summary>
    public class TenantOutput
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string AdminName { get; set; }

        /// <summary>
        /// 主机
        /// </summary>
        public string Host { get; set; }

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
        public string Connection { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeOffset CreatedTime { get; set; }
    }
}