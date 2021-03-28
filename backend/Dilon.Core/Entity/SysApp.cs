using Furion.Snowflake;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dilon.Core
{
    /// <summary>
    /// 系统应用表
    /// </summary>
    [Table("sys_app")]
    public class SysApp : DEntityBase
    {
        public SysApp()
        {
            Id = IDGenerator.NextId();
            CreatedTime = DateTimeOffset.Now;
            IsDeleted = false;
            Status = (int)CommonStatus.ENABLE;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 是否默认激活（Y-是，N-否）,只能有一个系统默认激活
        /// 用户登录后默认展示此系统菜单
        /// </summary>
        public string Active { get; set; }

        /// <summary>
        /// 状态（字典 0正常 1停用 2删除）
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
    }
}
