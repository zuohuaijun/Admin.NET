using SqlSugar;
using System;

namespace Dilon.Application
{
    /// <summary>
    /// SqlSugar实体
    /// </summary>
    [SugarTable("Test", TableDescription = "我的业务表")]
    public class Test
    {
        /// <summary>
        /// 雪花Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public long Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(ColumnDataType = "Nvarchar(32)")]
        public string Name { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTimeOffset CreateTime { get; set; }
    }
}
