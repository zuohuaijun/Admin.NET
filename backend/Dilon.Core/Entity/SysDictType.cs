using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dilon.Core
{
    /// <summary>
    /// 字典类型表
    /// </summary>
    [Table("sys_dict_type")]
    public class SysDictType : DEntityBase, IEntityTypeBuilder<SysDictType>
    {
        public SysDictType()
        {
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

        /// <summary>
        /// 字典数据
        /// </summary>
        public ICollection<SysDictData> SysDictDatas { get; set; }

        public void Configure(EntityTypeBuilder<SysDictType> entityBuilder, DbContext dbContext, Type dbContextLocator)
        {
            entityBuilder.HasMany(x => x.SysDictDatas).WithOne(x => x.SysDictType).HasForeignKey(x => x.TypeId);
        }
    }
}
