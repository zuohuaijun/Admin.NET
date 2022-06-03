using Admin.NET.Application.Const;

namespace Admin.NET.Application.Entity;

/// <summary>
/// 多库代码生成测试学生表
/// </summary>
[SugarTable("d_student", "多库代码生成测试学生表")]
[SqlSugarEntity(DbConfigId = TestConst.ConfigId)]
public class Student : EntityBase
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