using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Core
{
    /// <summary>
    /// 系统角色表
    /// </summary>
    [SugarTable("sys_role", "系统角色表")]
    [SqlSugarEntity]
    public class SysRole : EntityTenant
    {
        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(ColumnDescription = "名称", Length = 20)]
        [Required, MaxLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [SugarColumn(ColumnDescription = "编码", Length = 50)]
        [MaxLength(50)]
        public string Code { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [SugarColumn(ColumnDescription = "排序")]
        public int Order { get; set; }

        /// <summary>
        /// 数据范围（1全部数据 2本部门及以下数据 3本部门数据 4仅本人数据 5自定义数据）
        /// </summary>
        [SugarColumn(ColumnDescription = "数据范围")]
        public DataScopeEnum DataScope { get; set; } = DataScopeEnum.All;

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnDescription = "备注", Length = 100)]
        [MaxLength(100)]
        public string Remark { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [SugarColumn(ColumnDescription = "状态")]
        public StatusEnum Status { get; set; } = StatusEnum.Enable;
    }
}