namespace Admin.NET.Core.Service;

/// <summary>
/// 代码生成器服务
/// </summary>
[ApiDescriptionSettings(Name = "代码生成器", Order = 150)]
public class CodeGenService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SysCodeGen> _sysCodeGenRep;
    private readonly SqlSugarRepository<SysMenu> _sysMenuRep;
    private readonly CodeGenConfigService _codeGenConfigService;
    private readonly IViewEngine _viewEngine;
    private readonly ICommonService _commonService;

    private readonly ISqlSugarClient _db;

    public CodeGenService(SqlSugarRepository<SysCodeGen> sysCodeGenRep,
        SqlSugarRepository<SysMenu> sysMenuRep,
        CodeGenConfigService codeGenConfigService,
        IViewEngine viewEngine,
        ICommonService commonService,
        ISqlSugarClient db)
    {
        _sysCodeGenRep = sysCodeGenRep;
        _sysMenuRep = sysMenuRep;
        _codeGenConfigService = codeGenConfigService;
        _viewEngine = viewEngine;
        _commonService = commonService;
        _db = db;
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet("/codeGenerate/page")]
    public async Task<dynamic> QueryCodeGenPageList([FromQuery] CodeGenInput input)
    {
        var tableName = !string.IsNullOrEmpty(input.TableName?.Trim());
        var codeGens = await _sysCodeGenRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.TableName), u => u.TableName.Contains(input.TableName.Trim()))
            .ToPagedListAsync(input.Page, input.PageSize);
        return codeGens;
    }

    /// <summary>
    /// 增加
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/codeGenerate/add")]
    public async Task AddCodeGen(AddCodeGenInput input)
    {
        var isExist = await _sysCodeGenRep.AsQueryable().Where(u => u.TableName == input.TableName).AnyAsync();
        if (isExist)
            throw Oops.Oh(ErrorCodeEnum.D1400);

        var codeGen = input.Adapt<SysCodeGen>();
        var newCodeGen = await _sysCodeGenRep.Context.Insertable(codeGen).ExecuteReturnEntityAsync();
        // 加入配置表中
        _codeGenConfigService.AddList(GetColumnList(input), newCodeGen);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="inputs"></param>
    /// <returns></returns>
    [HttpPost("/codeGenerate/delete")]
    public async Task DeleteCodeGen(List<DeleteCodeGenInput> inputs)
    {
        if (inputs == null || inputs.Count < 1) return;

        var codeGenConfigTaskList = new List<Task>();
        inputs.ForEach(u =>
        {
            _sysCodeGenRep.DeleteById(u.Id);

            // 删除配置表中
            codeGenConfigTaskList.Add(_codeGenConfigService.Delete(u.Id));
        });
        await Task.WhenAll(codeGenConfigTaskList);
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/codeGenerate/edit")]
    public async Task UpdateCodeGen(UpdateCodeGenInput input)
    {
        var isExist = await _sysCodeGenRep.AsQueryable().AnyAsync(u => u.TableName == input.TableName && u.Id != input.Id);
        if (isExist)
            throw Oops.Oh(ErrorCodeEnum.D1400);

        var codeGen = input.Adapt<SysCodeGen>();
        await _sysCodeGenRep.UpdateAsync(codeGen);
    }

    /// <summary>
    /// 详情
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet("/codeGenerate/detail")]
    public async Task<SysCodeGen> GetCodeGen([FromQuery] QueryCodeGenInput input)
    {
        return await _sysCodeGenRep.AsQueryable().SingleAsync(m => m.Id == input.Id);
    }

    /// <summary>
    /// 获取数据库库集合
    /// </summary>
    /// <returns></returns>
    [HttpGet("codeGenerate/DatabaseList")]
    public async Task<List<ConnectionConfig>> GetDatabaseList()
    {
        return await Task.FromResult(App.GetOptions<ConnectionStringsOptions>().ConnectionConfigs);
    }

    /// <summary>
    /// 获取数据库表(实体)集合
    /// </summary>
    /// <returns></returns>
    [HttpGet("/codeGenerate/InformationList/{configId}")]
    public async Task<List<TableOutput>> GetTableList(string configId = SqlSugarConst.ConfigId)
    {
        //切库,多库代码生成用
        _db.AsTenant().ChangeDatabase(configId);
        List<DbTableInfo> dbTableInfos = _db.DbMaintenance.GetTableInfoList(false);//这里不能走缓存,否则切库不起作用

        List<string> dbTableNames = dbTableInfos.Select(x => x.Name).ToList();

        IEnumerable<EntityInfo> entityInfos = await _commonService.GetEntityInfos();
        entityInfos = entityInfos.Where(x => dbTableNames.Contains(x.DbTableName));
        var result = new List<TableOutput>();
        foreach (var item in entityInfos)
        {
            result.Add(new TableOutput()
            {
                ConfigId = configId,
                EntityName = item.EntityName,
                TableName = item.DbTableName,
                TableComment = item.TableDescription
            });
        }
        return result;
    }

    /// <summary>
    /// 根据表名获取列
    /// </summary>
    /// <returns></returns>
    [HttpGet("/codeGenerate/ColumnList/{configId}/{tableName}")]
    public List<TableColumnOuput> GetColumnListByTableName(string tableName, string configId = SqlSugarConst.ConfigId)
    {
        // 切库---多库代码生成用
        _db.AsTenant().ChangeDatabase(configId);

        // 获取实体类型属性
        var entityType = _db.DbMaintenance.GetTableInfoList(false).FirstOrDefault(u => u.Name == tableName);
        if (entityType == null) return null;
        // 按原始类型的顺序获取所有实体类型属性（不包含导航属性，会返回null）
        return _db.DbMaintenance.GetColumnInfosByTableName(entityType.Name).Select(u => new TableColumnOuput
        {
            ColumnName = u.DbColumnName,
            ColumnKey = u.IsPrimarykey.ToString(),
            DataType = u.DataType.ToString(),
            NetType = CodeGenUtil.ConvertDataType(u.DataType.ToString()),
            ColumnComment = u.ColumnDescription
        }).ToList();
    }

    /// <summary>
    /// 获取数据表列（实体属性）集合
    /// </summary>
    /// <returns></returns>
    [NonAction]
    public List<TableColumnOuput> GetColumnList([FromQuery] AddCodeGenInput input)
    {
        // 切库---多库代码生成用
        if (!string.IsNullOrEmpty(input.ConfigId))
            _db.AsTenant().ChangeDatabase(input.ConfigId);

        var entityType = _commonService.GetEntityInfos().Result.FirstOrDefault(m => m.EntityName == input.TableName);
        if (entityType == null)
            return null;

        return _db.DbMaintenance.GetColumnInfosByTableName(entityType.DbTableName, false).Select(u => new TableColumnOuput
        {
            ColumnName = u.DbColumnName,
            ColumnKey = u.IsPrimarykey.ToString(),
            DataType = u.DataType.ToString(),
            ColumnComment = string.IsNullOrWhiteSpace(u.ColumnDescription) ? u.DbColumnName : u.ColumnDescription
        }).ToList();
    }

    /// <summary>
    /// 代码生成_本地项目
    /// </summary>
    /// <returns></returns>
    [HttpPost("/codeGenerate/runLocal")]
    public async Task RunLocal(SysCodeGen input)
    {
        // 先删除该表已生成的菜单列表
        var templatePathList = GetTemplatePathList();
        var targetPathList = GetTargetPathList(input);
        for (var i = 0; i < templatePathList.Count; i++)
        {
            var tContent = File.ReadAllText(templatePathList[i]);

            var tableFieldList = await _codeGenConfigService.List(new CodeGenConfig() { CodeGenId = input.Id }); // 字段集合
            var queryWhetherList = tableFieldList.Where(u => u.QueryWhether == YesNoEnum.Y.ToString()).ToList(); // 前端查询集合
            var joinTableList = tableFieldList.Where(u => u.EffectType == "Upload" || u.EffectType == "fk").ToList();//需要连表查询的字段
            (string joinTableNames, string lowerJoinTableNames) = GetJoinTableStr(joinTableList);//获取连表的实体名和别名

            var data = new CustomViewEngine(_sysCodeGenRep)
            {
                ConfigId = input.ConfigId,
                AuthorName = input.AuthorName,
                BusName = input.BusName,
                NameSpace = input.NameSpace,
                ClassName = input.TableName,
                QueryWhetherList = queryWhetherList,
                TableField = tableFieldList,
                IsJoinTable = joinTableList.Count > 0,
                IsUpload = joinTableList.Where(u => u.EffectType == "Upload").Any(),
            };
            var tResult = _viewEngine.RunCompile<CustomViewEngine>(tContent, data, builderAction: builder =>
            {
                builder.AddAssemblyReferenceByName("System.Linq");
                builder.AddAssemblyReferenceByName("System.Collections");
                builder.AddUsing("System.Collections.Generic");
                builder.AddUsing("System.Linq");
            });
            var dirPath = new DirectoryInfo(targetPathList[i]).Parent.FullName;
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            File.WriteAllText(targetPathList[i], tResult, Encoding.UTF8);
        }

        await AddMenu(input.TableName, input.BusName, input.MenuPid);
    }

    /// <summary>
    /// 获取连表的实体名和别名
    /// </summary>
    /// <param name="configs"></param>
    /// <returns></returns>
    private (string, string) GetJoinTableStr(List<CodeGenConfig> configs)
    {
        var uploads = configs.Where(u => u.EffectType == "Upload").ToList();
        var fks = configs.Where(u => u.EffectType == "fk").ToList();
        string str = ""; //<Order, OrderItem, Custom>
        string lowerStr = ""; //(o, i, c)
        foreach (var item in uploads)
        {
            lowerStr += "sysFile_FK_" + item.LowerColumnName + ",";
            str += "SysFile,";
        }
        foreach (var item in fks)
        {
            lowerStr += item.LowerFkEntityName + "_FK_" + item.LowerFkColumnName + ",";
            str += item.FkEntityName + ",";
        }
        return (str.TrimEnd(','), lowerStr.TrimEnd(','));
    }

    private async Task AddMenu(string className, string busName, long pid)
    {
        // 如果 pid 为 0 说明为顶级菜单, 需要创建顶级目录
        if (pid == 0)
        {
            // 目录
            var menuType0 = new SysMenu
            {
                Pid = 0,
                Title = busName + "管理",
                Type = MenuTypeEnum.Dir,
                Icon = "robot",
                Path = "/" + className.ToLower(),
                Component = "LAYOUT",
            };
            pid = (await _sysMenuRep.Context.Insertable(menuType0).ExecuteReturnEntityAsync()).Id;
        }
        // 由于后续菜单会有修改, 需要判断下 pid 是否存在, 不存在报错
        else if (!await _sysMenuRep.AsQueryable().AnyAsync(e => e.Id == pid))
            throw Oops.Oh(ErrorCodeEnum.D1505);

        // 菜单
        var menuType1 = new SysMenu
        {
            Pid = pid,
            Title = busName + "管理",
            Name = className + "Management",
            Type = MenuTypeEnum.Menu,
            Path = "/" + className.ToLower(),
            Component = "/main/" + className + "/index",
        };
        var pid1 = (await _sysMenuRep.Context.Insertable(menuType1).ExecuteReturnEntityAsync()).Id;

        // 按钮-page
        var menuType2 = new SysMenu
        {
            Pid = pid1,
            Title = busName + "查询",
            Type = MenuTypeEnum.Btn,
            Permission = className + ":page",
        };

        // 按钮-detail
        var menuType2_1 = new SysMenu
        {
            Pid = pid1,
            Title = busName + "详情",
            Type = MenuTypeEnum.Btn,
            Permission = className + ":detail",
        };

        // 按钮-add
        var menuType2_2 = new SysMenu
        {
            Pid = pid1,
            Title = busName + "增加",
            Type = MenuTypeEnum.Btn,
            Permission = className + ":add",
        };

        // 按钮-delete
        var menuType2_3 = new SysMenu
        {
            Pid = pid1,
            Title = busName + "删除",
            Type = MenuTypeEnum.Btn,
            Permission = className + ":delete",
        };

        // 按钮-edit
        var menuType2_4 = new SysMenu
        {
            Pid = pid1,
            Title = busName + "编辑",
            Type = MenuTypeEnum.Btn,
            Permission = className + ":update",
        };

        var menuList = new List<SysMenu>() { menuType2, menuType2_1, menuType2_2, menuType2_3, menuType2_4 };
        await _sysMenuRep.Context.Insertable(menuList).ExecuteCommandAsync();
    }

    /// <summary>
    /// 获取模板文件路径集合
    /// </summary>
    /// <returns></returns>
    private static List<string> GetTemplatePathList()
    {
        var templatePath = App.WebHostEnvironment.WebRootPath + @"\Template\";
        return new List<string>()
        {
            Path.Combine(templatePath , "Service.cs.vm"),
            Path.Combine(templatePath , "Input.cs.vm"),
            Path.Combine(templatePath , "Output.cs.vm"),
            Path.Combine(templatePath , "Dto.cs.vm"),
            Path.Combine(templatePath , "index.vue.vm"),
            Path.Combine(templatePath , "data.data.ts.vm"),
            Path.Combine(templatePath , "dataModal.vue.vm"),
            Path.Combine(templatePath , "manage.js.vm"),
        };
    }

    /// <summary>
    /// 设置生成文件路径
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private static List<string> GetTargetPathList(SysCodeGen input)
    {
        var backendPath = Path.Combine(new DirectoryInfo(App.WebHostEnvironment.ContentRootPath).Parent.FullName, "Admin.NET.Application", "Service", input.TableName);
        var servicePath = Path.Combine(backendPath, input.TableName + "Service.cs");
        var inputPath = Path.Combine(backendPath, "Dto", input.TableName + "Input.cs");
        var outputPath = Path.Combine(backendPath, "Dto", input.TableName + "Output.cs");
        var viewPath = Path.Combine(backendPath, "Dto", input.TableName + "Dto.cs");
        var frontendPath = Path.Combine(new DirectoryInfo(App.WebHostEnvironment.ContentRootPath).Parent.Parent.FullName, "Vben2", "src", "views", "main");
        var indexPath = Path.Combine(frontendPath, input.TableName, "index.vue");
        var formDataPath = Path.Combine(frontendPath, input.TableName, "data.data.ts");
        var formModalPath = Path.Combine(frontendPath, input.TableName, "dataModal.vue");
        var apiJsPath = Path.Combine(new DirectoryInfo(App.WebHostEnvironment.ContentRootPath).Parent.Parent.FullName, "Vben2", "src", "api", "main", input.TableName + ".ts");

        return new List<string>()
        {
            servicePath,
            inputPath,
            outputPath,
            viewPath,
            indexPath,
            formDataPath,
            formModalPath,
            apiJsPath
        };
    }
}