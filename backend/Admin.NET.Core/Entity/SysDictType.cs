using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Admin.NET.Core
{
    /// <summary>
    /// 字典类型表
    /// </summary>
    [Table("sys_dict_type")]
    [Comment("字典类型表")]
    public class SysDictType : DEntityBase, IEntityTypeBuilder<SysDictType>
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Comment("名称")]
        [Required, MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [Comment("编码")]
        [Required, MaxLength(50)]
        public string Code { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Comment("排序")]
        public int Sort { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Comment("备注")]
        [MaxLength(100)]
        public string Remark { get; set; }

        /// <summary>
        /// 状态（字典 0正常 1停用 2删除）
        /// </summary>
        [Comment("状态")]
        public CommonStatus Status { get; set; } = CommonStatus.ENABLE;

        /// <summary>
        /// 字典数据
        /// </summary>
        public ICollection<SysDictData> SysDictDatas { get; set; }

        public void Configure(EntityTypeBuilder<SysDictType> entityBuilder, DbContext dbContext, Type dbContextLocator)
        {
            entityBuilder.HasMany(x => x.SysDictDatas)
                .WithOne(x => x.SysDictType)
                .HasForeignKey(x => x.TypeId);
        }
    }
}