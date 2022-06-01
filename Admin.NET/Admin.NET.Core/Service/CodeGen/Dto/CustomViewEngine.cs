namespace Admin.NET.Core.Service;

public class CustomViewEngine : ViewEngineModel
{
    private readonly SqlSugarRepository<SysCodeGen> _sysCodeGenRep; // 代码生成器仓储

    public CustomViewEngine()
    {
    }

    public CustomViewEngine(SqlSugarRepository<SysCodeGen> sysCodeGenRep)
    {
        _sysCodeGenRep = sysCodeGenRep;
    }

    /// <summary>
    /// 库定位器
    /// </summary>
    public string DbConfigId { get; set; } = SqlSugarConst.ConfigId;

    public string AuthorName { get; set; }

    public string BusName { get; set; }

    public string NameSpace { get; set; }

    public string ClassName { get; set; }

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
    private List<TableColumnOuput> ColumnList { get; set; }

    public string GetColumnNetType(object tbName, object colName)
    {
        ColumnList = GetColumnListByTableName(tbName.ToString());
        var col = ColumnList.Where(c => c.ColumnName == colName.ToString()).FirstOrDefault();
        //多库代码生成切库调用后切换回原库
        _sysCodeGenRep.Context.AsTenant().ChangeDatabase(SqlSugarConst.ConfigId);
        return col.NetType;
    }

    public List<TableColumnOuput> GetColumnListByTableName(string tableName)
    {
        //多库代码生成切换库
        if (DbConfigId != SqlSugarConst.ConfigId)
            _sysCodeGenRep.Context.AsTenant().ChangeDatabase(DbConfigId);

        // 获取实体类型属性
        var entityType = _sysCodeGenRep.Context.DbMaintenance.GetTableInfoList().FirstOrDefault(u => u.Name == tableName);
        if (entityType == null) return null;

        // 按原始类型的顺序获取所有实体类型属性（不包含导航属性，会返回null）
        return _sysCodeGenRep.Context.DbMaintenance.GetColumnInfosByTableName(entityType.Name).Select(u => new TableColumnOuput
        {
            ColumnName = u.DbColumnName,
            ColumnKey = u.IsPrimarykey.ToString(),
            DataType = u.DataType.ToString(),
            NetType = CodeGenUtil.ConvertDataType(u.DataType.ToString()),
            ColumnComment = u.ColumnDescription
        }).ToList();
    }
}