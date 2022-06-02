using Admin.NET.Application.Const;

namespace Admin.NET.Application.Entity;

/// <summary>
/// 多库代码生成树形测试表
/// </summary>
[SugarTable("d_treetest", "多库代码生成树形测试表")]
[SqlSugarEntity(DbConfigId = TestConst.ConfigId)]
public class TreeTest : EntityBase
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 父级
    /// </summary>
    public long ParentId { get; set; }//父级字段

    /// <summary>
    /// Child
    /// </summary>
    [SqlSugar.SugarColumn(IsIgnore = true)]
    public List<TreeTest> Child { get; set; }
}
