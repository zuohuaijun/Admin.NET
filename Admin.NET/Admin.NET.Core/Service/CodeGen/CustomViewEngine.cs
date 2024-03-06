// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

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
    public string ConfigId { get; set; } = SqlSugarConst.MainConfigId;

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

    public bool IsJoinTable { get; set; }

    public bool IsUpload { get; set; }

    public string PrintType { get; set; }

    public string PrintName { get; set; }

    public List<CodeGenConfig> QueryWhetherList { get; set; }

    public List<CodeGenConfig> TableField { get; set; }

    private List<ColumnOuput> ColumnList { get; set; }

    public string GetColumnNetType(object tbName, object colName)
    {
        var config = App.GetOptions<DbConnectionOptions>().ConnectionConfigs.FirstOrDefault(u => u.ConfigId.ToString() == ConfigId);
        ColumnList = GetColumnListByTableName(tbName.ToString());
        var col = ColumnList.Where(c => (config.DbSettings.EnableUnderLine
            ? CodeGenUtil.CamelColumnName(c.ColumnName, Array.Empty<string>())
            : c.ColumnName) == colName.ToString()).FirstOrDefault();
        return col.NetType;
    }

    public List<ColumnOuput> GetColumnListByTableName(string tableName)
    {
        // 多库代码生成切换库
        var provider = _db.AsTenant().GetConnectionScope(ConfigId != SqlSugarConst.MainConfigId ? ConfigId : SqlSugarConst.MainConfigId);

        // 获取实体类型属性
        var entityType = provider.DbMaintenance.GetTableInfoList().FirstOrDefault(u => u.Name == tableName);

        // 因为ConfigId的表通常也会用到主库的表来做连接，所以这里如果在ConfigId中找不到实体也尝试一下在主库中查找
        if (ConfigId == SqlSugarConst.MainConfigId && entityType == null) return null;
        if (ConfigId != SqlSugarConst.MainConfigId)
        {
            provider = _db.AsTenant().GetConnectionScope(SqlSugarConst.MainConfigId);
            entityType = provider.DbMaintenance.GetTableInfoList().FirstOrDefault(u => u.Name == tableName);
            if (entityType == null) return null;
        }

        // 按原始类型的顺序获取所有实体类型属性（不包含导航属性，会返回null）
        return provider.DbMaintenance.GetColumnInfosByTableName(entityType.Name).Select(u => new ColumnOuput
        {
            ColumnName = u.DbColumnName,
            ColumnKey = u.IsPrimarykey.ToString(),
            DataType = u.DataType.ToString(),
            NetType = CodeGenUtil.ConvertDataType(u, provider.CurrentConnectionConfig.DbType),
            ColumnComment = u.ColumnDescription
        }).ToList();
    }
}