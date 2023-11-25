// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

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

    public List<CodeGenConfig> QueryWhetherList { get; set; }

    public List<CodeGenConfig> TableField { get; set; }

    public bool IsJoinTable { get; set; }

    public bool IsUpload { get; set; }

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