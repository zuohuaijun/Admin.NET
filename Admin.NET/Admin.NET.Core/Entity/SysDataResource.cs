using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Admin.NET.Core;
using SqlSugar;
namespace Admin.NET.Core
{
    /// <summary>
    ///  数据资源表
    ///</summary>
    [SugarTable("sys_data_resource", "数据资源表")]
    [SqlSugarEntity]
    public class SysDataResource : EntityBase
    {
        /// <summary>
        /// 父节点Id 
        ///</summary>
        [SugarColumn(ColumnName = "pid", ColumnDescription = "父Id")]
        public long Pid { get; set; }
        /// <summary>
        /// 名称 
        ///</summary>
        [SugarColumn(ColumnName = "name", ColumnDescription = "名称", Length = 200)]
        [MaxLength(200)]
        public string Name { get; set; }
        /// <summary>
        /// 值 
        ///</summary>
        [SugarColumn(ColumnName = "value", ColumnDescription = "值", Length = 200)]
        [MaxLength(200)]
        public string Value { get; set; }
        /// <summary>
        /// 节点编码 
        ///</summary>
        [SugarColumn(ColumnName = "code", ColumnDescription = "节点编码", Length = 100)]
        [MaxLength(100)]
        public string Code { get; set; }
        /// <summary>
        /// 排序 
        ///</summary>
        [SugarColumn(ColumnName = "order", ColumnDescription = "排序")]
        public int Order { get; set; }
        /// <summary>
        /// 备注 
        ///</summary>
        [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", Length = 200)]
        [MaxLength(200)]
        public string Remark { get; set; }
        /// <summary>
        /// 状态：是否启用 
        ///</summary>
        [SugarColumn(ColumnName = "status", ColumnDescription = "状态")]
        public StatusEnum Status { get; set; } = StatusEnum.Enable;

        /// <summary>
        /// 数据资源子项
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<SysDataResource> Children { get; set; }
    }
}
