namespace Admin.NET.Core.Service;

/// <summary>
/// 数据库管理服务
/// </summary>
[ApiDescriptionSettings(Name = "数据库管理", Order = 145)]
public class DataBaseManager : IDynamicApiController, ITransient
{
    private readonly ISqlSugarClient _sqlSugarClient;
    private readonly IViewEngine _viewEngine;

    public DataBaseManager(ISqlSugarClient sqlSugarClient, IViewEngine viewEngine)
    {
        _sqlSugarClient = sqlSugarClient;
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
        _sqlSugarClient.DbMaintenance.AddColumn(input.TableName, column);
        _sqlSugarClient.DbMaintenance.AddColumnRemark(input.DbColumnName, input.TableName, input.ColumnDescription);
        if (column.IsPrimarykey)
        {
            _sqlSugarClient.DbMaintenance.AddPrimaryKey(input.TableName, input.DbColumnName);
        }
    }

    /// <summary>
    /// 删除列
    /// </summary>
    /// <param name="input"></param>
    [HttpPost("/column/delete")]
    public void ColumnDelete(DbColumnInfoOutput input)
    {
        _sqlSugarClient.DbMaintenance.DropColumn(input.TableName, input.DbColumnName);
    }

    /// <summary>
    /// 编辑列
    /// </summary>
    /// <param name="input"></param>
    [HttpPost("/column/edit")]
    public void ColumnEdit(EditColumnInput input)
    {
        _sqlSugarClient.DbMaintenance.RenameColumn(input.TableName, input.OldName, input.DbColumnName);
        if (_sqlSugarClient.DbMaintenance.IsAnyColumnRemark(input.DbColumnName, input.TableName))
        {
            _sqlSugarClient.DbMaintenance.DeleteColumnRemark(input.DbColumnName, input.TableName);
        }
        _sqlSugarClient.DbMaintenance.AddColumnRemark(input.DbColumnName, input.TableName, string.IsNullOrWhiteSpace(input.ColumnDescription) ? input.DbColumnName : input.ColumnDescription);
    }

    /// <summary>
    /// 获取表字段
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    [HttpGet("/dataBase/columnInfoList")]
    public List<DbColumnInfoOutput> GetColumnInfosByTableName(string tableName)
    {
        if (string.IsNullOrWhiteSpace(tableName))
            return new List<DbColumnInfoOutput>();
        return _sqlSugarClient.DbMaintenance.GetColumnInfosByTableName(tableName, false).Adapt<List<DbColumnInfoOutput>>();
    }

    /// <summary>
    /// 获取所有表
    /// </summary>
    /// <returns></returns>
    [HttpGet("/dataBase/tableInfoList")]
    public List<DbTableInfo> GetTableInfoList()
    {
        return _sqlSugarClient.DbMaintenance.GetTableInfoList(false);
    }

    /// <summary>
    /// 新增表
    /// </summary>
    /// <param name="input"></param>
    [HttpPost("/table/add")]
    public void TableAdd(DbTableInfoInput input)
    {
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
        _sqlSugarClient.DbMaintenance.CreateTable(input.Name, columns, false);
        _sqlSugarClient.DbMaintenance.AddTableRemark(input.Name, input.Description);
        if (columns.Any(m => m.IsPrimarykey))
        {
            _sqlSugarClient.DbMaintenance.AddPrimaryKey(input.Name, columns.FirstOrDefault(m => m.IsPrimarykey).DbColumnName);
        }
        input.DbColumnInfoList.ForEach(m =>
        {
            _sqlSugarClient.DbMaintenance.AddColumnRemark(m.DbColumnName, input.Name, string.IsNullOrWhiteSpace(m.ColumnDescription) ? m.DbColumnName : m.ColumnDescription);
        });
    }

    /// <summary>
    /// 删除表
    /// </summary>
    /// <param name="input"></param>
    [HttpPost("/table/delete")]
    public void TableDelete(DbTableInfo input)
    {
        _sqlSugarClient.DbMaintenance.DropTable(input.Name);
    }

    /// <summary>
    /// 编辑表
    /// </summary>
    /// <param name="input"></param>
    [HttpPost("/table/edit")]
    public void TableEdit(EditTableInput input)
    {
        _sqlSugarClient.DbMaintenance.RenameTable(input.OldName, input.Name);
        if (_sqlSugarClient.DbMaintenance.IsAnyTableRemark(input.Name))
        {
            _sqlSugarClient.DbMaintenance.DeleteTableRemark(input.Name);
        }
        _sqlSugarClient.DbMaintenance.AddTableRemark(input.Name, input.Description);
    }

    /// <summary>
    /// 创建实体
    /// </summary>
    /// <param name="input"></param>
    [HttpPost("/table/createEntity")]
    public void CreateEntity(CreateEntityInput input)
    {
        input.Position = string.IsNullOrWhiteSpace(input.Position) ? "Magic.Application" : input.Position;
        input.BaseClassName = string.IsNullOrWhiteSpace(input.BaseClassName) ? "" : $" : {input.BaseClassName}";
        var templatePath = GetTemplatePath();
        var targetPath = GetTargetPath(input);
        DbTableInfo dbTableInfo = _sqlSugarClient.DbMaintenance.GetTableInfoList(false).FirstOrDefault(m => m.Name == input.TableName);
        if (dbTableInfo == null)
            throw Oops.Oh(ErrorCodeEnum.db1001);
        List<DbColumnInfo> dbColumnInfos = _sqlSugarClient.DbMaintenance.GetColumnInfosByTableName(input.TableName, false);

        if (input.BaseClassName.Contains("DBEntityTenant"))
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
            m.DataType = CodeGenUtil.ConvertDataType(m.DataType, App.GetOptions<ConnectionStringsOptions>().DefaultDbType);
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