using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dilon.Application
{

    [SugarTable("Test", TableDescription = "测试表")]
    public class Test
    {
        [SugarColumn(IsPrimaryKey = true)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        [SugarColumn(ColumnDataType = "Nvarchar(255)")]//自定格式的情况 length不要设置
        public string Text { get; set; }
        [SugarColumn(IsNullable = true)]
        public DateTime CreateTime { get; set; }


    }
}
