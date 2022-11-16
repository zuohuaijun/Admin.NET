namespace Admin.NET.Core.Service;

public class CustomViewEngine : ViewEngineModel
{
    private readonly ISqlSugarClient _db;

    public CustomViewEngine()
    {
    }

    public CustomViewEngine(ISqlSugarClient db)
    {
        _db = db;
    }

    /// <summary>
    /// 库定位器
    /// </summary>
    public string ConfigId { get; set; } = SqlSugarConst.ConfigId;

    public string AuthorName { get; set; }

    public string BusName { get; set; }

    public string NameSpace { get; set; }

    public string ClassName { get; set; }

    public string ProjectLastName { get; set; }

    public string LowerClassName
    {
        get
        {
            return ClassName[..1].ToLower() + ClassName[1..]; // 首字母小写
        }
    }

    public List<CodeGenConfig> QueryWhetherList { get; set; }

    public List<CodeGenConfig> TableField { get; set; }

    public bool IsJoinTable { get; set; }

    public bool IsUpload { get; set; }

    private List<ColumnOuput> ColumnList { get; set; }

    public string GetColumnNetType(object tbName, object colName)
    {
        ColumnList = GetColumnListByTableName(tbName.ToString());
        var col = ColumnList.Where(c => c.ColumnName == colName.ToString()).FirstOrDefault();
        return col.NetType;
    }

    public List<ColumnOuput> GetColumnListByTableName(string tableName)
    {
        // 多库代码生成切换库
        var provider = _db.AsTenant().GetConnectionScope(ConfigId != SqlSugarConst.ConfigId ? ConfigId : SqlSugarConst.ConfigId);

        // 获取实体类型属性
        var entityType = provider.DbMaintenance.GetTableInfoList().FirstOrDefault(u => u.Name == tableName);
        if (entityType == null) return null;

        // 按原始类型的顺序获取所有实体类型属性（不包含导航属性，会返回null）
        return provider.DbMaintenance.GetColumnInfosByTableName(entityType.Name).Select(u => new ColumnOuput
        {
            ColumnName = u.DbColumnName,
            ColumnKey = u.IsPrimarykey.ToString(),
            DataType = u.DataType.ToString(),
            NetType = CodeGenUtil.ConvertDataType(u.DataType.ToString()),
            ColumnComment = u.ColumnDescription
        }).ToList();
    }
}