namespace Admin.NET.Core.Service;

/// <summary>
/// 系统数据库管理服务
/// </summary>
[ApiDescriptionSettings(Order = 145)]
public class SysDataBaseService : IDynamicApiController, ITransient
{
    private readonly ISqlSugarClient _db;
    private readonly IViewEngine _viewEngine;

    public SysDataBaseService(ISqlSugarClient db, IViewEngine viewEngine)
    {
        _db = db;
        _viewEngine = viewEngine;
    }

    /// <summary>
    /// 添加列
    /// </summary>
    /// <param name="input"></param>
    [HttpPost("/column/add")]
    public void ColumnAdd(DbColumnInfoInput input)
    {
        var column = new DbColumnInfo
        {
            ColumnDescription = input.ColumnDescription,
            DbColumnName = input.DbColumnName,
            IsIdentity = input.IsIdentity == 1,
            IsNullable = input.IsNullable == 1,
            IsPrimarykey = input.IsPrimarykey == 1,
            Length = input.Length,
            DecimalDigits = input.DecimalDigits,
            DataType = input.DataType
        };
        _db.DbMaintenance.AddColumn(input.TableName, column);
        _db.DbMaintenance.AddColumnRemark(input.DbColumnName, input.TableName, input.ColumnDescription);
        if (column.IsPrimarykey)
        {
            _db.DbMaintenance.AddPrimaryKey(input.TableName, input.DbColumnName);
        }
    }

    /// <summary>
    /// 删除列
    /// </summary>
    /// <param name="input"></param>
    [HttpPost("/column/delete")]
    public void ColumnDelete(DbColumnInfoOutput input)
    {
        _db.DbMaintenance.DropColumn(input.TableName, input.DbColumnName);
    }

    /// <summary>
    /// 编辑列
    /// </summary>
    /// <param name="input"></param>
    [HttpPost("/column/edit")]
    public void ColumnEdit(EditColumnInput input)
    {
        _db.DbMaintenance.RenameColumn(input.TableName, input.OldName, input.DbColumnName);
        if (_db.DbMaintenance.IsAnyColumnRemark(input.DbColumnName, input.TableName))
        {
            _db.DbMaintenance.DeleteColumnRemark(input.DbColumnName, input.TableName);
        }
        _db.DbMaintenance.AddColumnRemark(input.DbColumnName, input.TableName, string.IsNullOrWhiteSpace(input.ColumnDescription) ? input.DbColumnName : input.ColumnDescription);
    }

    /// <summary>
    /// 获取表字段
    /// </summary>
    /// <param name="tableName">表名</param>
    /// <param name="configId">ConfigId</param>
    /// <returns></returns>
    [HttpGet("/dataBase/columnInfoList")]
    public List<DbColumnInfoOutput> GetColumnInfosByTableName(string tableName, string configId = SqlSugarConst.ConfigId)
    {
        var provider = _db.AsTenant().GetConnectionScope(configId);
        if (string.IsNullOrWhiteSpace(tableName))
            return new List<DbColumnInfoOutput>();

        return provider.DbMaintenance.GetColumnInfosByTableName(tableName, false).Adapt<List<DbColumnInfoOutput>>();
    }

    /// <summary>
    /// 获取表信息
    /// </summary>
    /// <param name="configId">ConfigId</param>
    /// <returns></returns>
    [HttpGet("/dataBase/tableInfoList")]
    public List<DbTableInfo> GetTableInfoList(string configId = SqlSugarConst.ConfigId)
    {
        var provider = _db.AsTenant().GetConnectionScope(configId);
        return provider.DbMaintenance.GetTableInfoList(false);
    }

    /// <summary>
    /// 新增表
    /// </summary>
    /// <param name="input"></param>
    [HttpPost("/table/add")]
    public void TableAdd(DbTableInfoInput input)
    {
        var provider = _db.AsTenant().GetConnectionScope(input.ConfigId);
        var columns = new List<DbColumnInfo>();
        if (input.DbColumnInfoList == null || !input.DbColumnInfoList.Any())
        {
            throw Oops.Oh(ErrorCodeEnum.db1000);
        }
        input.DbColumnInfoList.ForEach(m =>
        {
            columns.Add(new DbColumnInfo
            {
                DbColumnName = m.DbColumnName.Trim(),
                DataType = m.DataType,
                Length = m.Length,
                ColumnDescription = m.ColumnDescription,
                IsNullable = m.IsNullable == 1,
                IsIdentity = m.IsIdentity == 1,
                IsPrimarykey = m.IsPrimarykey == 1,
                DecimalDigits = m.DecimalDigits
            });
        });
        provider.DbMaintenance.CreateTable(input.Name, columns, false);
        provider.DbMaintenance.AddTableRemark(input.Name, input.Description);
        if (columns.Any(m => m.IsPrimarykey))
        {
            provider.DbMaintenance.AddPrimaryKey(input.Name, columns.FirstOrDefault(m => m.IsPrimarykey).DbColumnName);
        }
        input.DbColumnInfoList.ForEach(m =>
        {
            provider.DbMaintenance.AddColumnRemark(m.DbColumnName, input.Name, string.IsNullOrWhiteSpace(m.ColumnDescription) ? m.DbColumnName : m.ColumnDescription);
        });
    }

    /// <summary>
    /// 删除表
    /// </summary>
    /// <param name="input"></param>
    [HttpPost("/table/delete")]
    public void TableDelete(DeleteTableInput input)
    {
        var provider = _db.AsTenant().GetConnectionScope(input.ConfigId);
        provider.DbMaintenance.DropTable(input.Name);
    }

    /// <summary>
    /// 编辑表
    /// </summary>
    /// <param name="input"></param>
    [HttpPost("/table/edit")]
    public void TableEdit(EditTableInput input)
    {
        var provider = _db.AsTenant().GetConnectionScope(input.ConfigId);
        provider.DbMaintenance.RenameTable(input.OldName, input.Name);
        if (provider.DbMaintenance.IsAnyTableRemark(input.Name))
        {
            provider.DbMaintenance.DeleteTableRemark(input.Name);
        }
        provider.DbMaintenance.AddTableRemark(input.Name, input.Description);
    }

    /// <summary>
    /// 创建实体
    /// </summary>
    /// <param name="input"></param>
    [HttpPost("/table/createEntity")]
    public void CreateEntity(CreateEntityInput input)
    {
        var provider = _db.AsTenant().GetConnectionScope(input.ConfigId);
        input.Position = string.IsNullOrWhiteSpace(input.Position) ? "Admin.NET.Application" : input.Position;
        input.BaseClassName = string.IsNullOrWhiteSpace(input.BaseClassName) ? "" : $" : {input.BaseClassName}";
        var templatePath = GetTemplatePath();
        var targetPath = GetTargetPath(input);
        DbTableInfo dbTableInfo = provider.DbMaintenance.GetTableInfoList(false).FirstOrDefault(m => m.Name == input.TableName);
        if (dbTableInfo == null)
            throw Oops.Oh(ErrorCodeEnum.db1001);
        List<DbColumnInfo> dbColumnInfos = provider.DbMaintenance.GetColumnInfosByTableName(input.TableName, false);

        if (input.BaseClassName.Contains("EntityTenant"))
        {
            dbColumnInfos = dbColumnInfos.Where(c => c.DbColumnName != "Id"
                && c.DbColumnName != "TenantId"
                && c.DbColumnName != "CreateTime"
                && c.DbColumnName != "UpdateTime"
                && c.DbColumnName != "CreateUserId"
                && c.DbColumnName != "UpdateUserId"
                && c.DbColumnName != "IsDelete").ToList();
        }
        else if (input.BaseClassName.Contains("EntityBase"))
        {
            dbColumnInfos = dbColumnInfos.Where(c => c.DbColumnName != "Id"
                && c.DbColumnName != "CreateTime"
                && c.DbColumnName != "UpdateTime"
                && c.DbColumnName != "CreateUserId"
                && c.DbColumnName != "UpdateUserId"
                && c.DbColumnName != "IsDelete").ToList();
        }
        else if (input.BaseClassName.Contains("EntityBaseId"))
        {
            dbColumnInfos = dbColumnInfos.Where(c => c.DbColumnName != "Id").ToList();
        }
        dbColumnInfos.ForEach(m =>
        {
            m.DataType = CodeGenUtil.ConvertDataType(m.DataType);
        });
        var tContent = File.ReadAllText(templatePath);
        var tResult = _viewEngine.RunCompileFromCached(tContent, new
        {
            input.TableName,
            input.EntityName,
            input.BaseClassName,
            dbTableInfo.Description,
            TableField = dbColumnInfos
        });
        File.WriteAllText(targetPath, tResult, Encoding.UTF8);
    }

    /// <summary>
    /// 获取模板文件路径集合
    /// </summary>
    /// <returns></returns>
    private static string GetTemplatePath()
    {
        var templatePath = App.WebHostEnvironment.WebRootPath + @"\Template\";
        return Path.Combine(templatePath, "Entity.cs.vm");
    }

    /// <summary>
    /// 设置生成文件路径
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private static string GetTargetPath(CreateEntityInput input)
    {
        var backendPath = Path.Combine(new DirectoryInfo(App.WebHostEnvironment.ContentRootPath).Parent.FullName, input.Position, "Entity");
        var entityPath = Path.Combine(backendPath, input.EntityName + ".cs");
        return entityPath;
    }
}