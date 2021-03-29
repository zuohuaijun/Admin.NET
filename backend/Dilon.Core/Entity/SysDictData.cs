﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Dilon.Core
{
    /// <summary>
    /// 字典值表
    /// </summary>
    [Table("sys_dict_data")]
    public class SysDictData : DEntityBase
    {
        /// <summary>
        /// 字典类型Id
        /// </summary>
        public long TypeId { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

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
        public CommonStatus Status { get; set; } = CommonStatus.ENABLE;

        /// <summary>
        /// 所属类型
        /// </summary>
        public SysDictType SysDictType { get; set; }
    }
}
