namespace Admin.NET.Application.Entity;

/// <summary>
/// 自己业务数据表
/// </summary>
[SugarTable("d_test", "自己业务数据表")]
[SqlSugarEntity] // [SqlSugarEntity(DbConfigId = TestConst.ConfigId)]
public class Test : EntityBase
{
    /// <summary>
    /// 姓名
    /// </summary>
    [SugarColumn(ColumnDescription = "姓名", Length = 20)]
    [Required, MaxLength(20)]
    public string Name { get; set; }

    /// <summary>
    /// 年龄
    /// </summary>
    [SugarColumn(ColumnDescription = "年龄")]
    public int Age { get; set; }

    /// <summary>
    /// 出生日期
    /// </summary>
    [SugarColumn(ColumnDescription = "出生日期")]
    public DateTime BirthDate { get; set; }
}
