using Dilon.Core.Service;
using Furion;
using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dilon.Core
{
    /// <summary>
    /// 参数配置表
    /// </summary>
    [Table("sys_config")]
    [Comment("参数配置表")]
    public class SysConfig : DEntityBase, IEntityChangedListener<SysConfig>
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Comment("名称")]
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [Comment("编码")]
        public string Code { get; set; }

        /// <summary>
        /// 属性值
        /// </summary>
        [Comment("属性值")]
        public string Value { get; set; }

        /// <summary>
        /// 是否是系统参数（Y-是，N-否）
        /// </summary>
        [Comment("是否是系统参数")]
        public string SysFlag { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Comment("备注")]
        public string Remark { get; set; }

        /// <summary>
        /// 状态（字典 0正常 1停用 2删除）
        /// </summary>
        [Comment("状态")]
        public CommonStatus Status { get; set; } = CommonStatus.ENABLE;

        /// <summary>
        /// 常量所属分类的编码，来自于“常量的分类”字典
        /// </summary>
        [Comment("常量所属分类的编码")]
        public string GroupCode { get; set; }

        /// <summary>
        /// 监听实体更改之后
        /// </summary>
        /// <param name="newEntity"></param>
        /// <param name="oldEntity"></param>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        /// <param name="state"></param>
        public void OnChanged(SysConfig newEntity, SysConfig oldEntity, DbContext dbContext, Type dbContextLocator, EntityState state)
        {
            // 刷新配置缓存
            App.GetService<ISysConfigService>().UpdateConfigCache(newEntity.Code, newEntity.Value);
        }
    }
}
