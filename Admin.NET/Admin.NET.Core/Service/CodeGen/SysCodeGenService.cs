namespace Admin.NET.Core.Service;

/// <summary>
/// 系统代码生成器服务
/// </summary>
[ApiDescriptionSettings(Order = 270)]
public class SysCodeGenService : IDynamicApiController, ITransient
{
    private readonly ISqlSugarClient _db;

    private readonly SysCodeGenConfigService _codeGenConfigService;
    private readonly IViewEngine _viewEngine;
    private readonly ICommonService _commonService;
    private readonly CodeGenOptions _codeGenOptions;

    public SysCodeGenService(ISqlSugarClient db,
        SysCodeGenConfigService codeGenConfigService,
        IViewEngine viewEngine,
        ICommonService commonService,
        IOptions<CodeGenOptions> codeGenOptions)
    {
        _db = db;
        _codeGenConfigService = codeGenConfigService;
        _viewEngine = viewEngine;
        _commonService = commonService;
        _codeGenOptions = codeGenOptions.Value;
    }

    /// <summary>
    /// 获取代码生成分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Page")]
    [DisplayName("获取代码生成分页列表")]
    public async Task<SqlSugarPagedList<SysCodeGen>> GetPage([FromQuery] CodeGenInput input)
    {
        return await _db.Queryable<SysCodeGen>()
            .WhereIF(!string.IsNullOrWhiteSpace(input.TableName), u => u.TableName.Contains(input.TableName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.BusName), u => u.BusName.Contains(input.BusName.Trim()))
            .ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加代码生成
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Add")]
    [DisplayName("增加代码生成")]
    public async Task AddCodeGen(AddCodeGenInput input)
    {
        var isExist = await _db.Queryable<SysCodeGen>().Where(u => u.TableName == input.TableName).AnyAsync();
        if (isExist)
            throw Oops.Oh(ErrorCodeEnum.D1400);

        var codeGen = input.Adapt<SysCodeGen>();
        var newCodeGen = await _db.Insertable(codeGen).ExecuteReturnEntityAsync();
        // 加入配置表中
        _codeGenConfigService.AddList(GetColumnList(input), newCodeGen);
    }

    /// <summary>
    /// 更新代码生成
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Update")]
    [DisplayName("更新代码生成")]
    public async Task UpdateCodeGen(UpdateCodeGenInput input)
    {
        var isExist = await _db.Queryable<SysCodeGen>().AnyAsync(u => u.TableName == input.TableName && u.Id != input.Id);
        if (isExist)
            throw Oops.Oh(ErrorCodeEnum.D1400);

        await _db.Updateable(input.Adapt<SysCodeGen>()).ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除代码生成
    /// </summary>
    /// <param name="inputs"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Delete")]
    [DisplayName("删除代码生成")]
    public async Task DeleteCodeGen(List<DeleteCodeGenInput> inputs)
    {
        if (inputs == null || inputs.Count < 1) return;

        var codeGenConfigTaskList = new List<Task>();
        inputs.ForEach(u =>
        {
            _db.Deleteable<SysCodeGen>().In(u.Id).ExecuteCommand();

            // 删除配置表中
            codeGenConfigTaskList.Add(_codeGenConfigService.DeleteCodeGenConfig(u.Id));
        });
        await Task.WhenAll(codeGenConfigTaskList);
    }

    /// <summary>
    /// 获取代码生成详情
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Detail")]
    [DisplayName("获取代码生成详情")]
    public async Task<SysCodeGen> GetDetail([FromQuery] QueryCodeGenInput input)
    {
        return await _db.Queryable<SysCodeGen>().SingleAsync(m => m.Id == input.Id);
    }

    /// <summary>
    /// 获取数据库库集合
    /// </summary>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "DatabaseList")]
    [DisplayName("获取数据库库集合")]
    public async Task<List<DatabaseOutput>> GetDatabaseList()
    {
        var dbCongigs = App.GetOptions<DbConnectionOptions>().ConnectionConfigs;
        return await Task.FromResult(dbCongigs.Adapt<List<DatabaseOutput>>());
    }

    /// <summary>
    /// 获取数据库表(实体)集合
    /// </summary>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "TableList")]
    [DisplayName("获取数据库表(实体)集合")]
    public async Task<List<TableOutput>> GetTableList(string configId = SqlSugarConst.ConfigId)
    {
        var provider = _db.AsTenant().GetConnectionScope(configId);
        var dbTableInfos = provider.DbMaintenance.GetTableInfoList(false); // 不能走缓存,否则切库不起作用

        var config = App.GetOptions<DbConnectionOptions>().ConnectionConfigs.FirstOrDefault(u => u.ConfigId == configId);

        var dbTableNames = dbTableInfos.Select(u => u.Name.ToLower()).ToList();
        IEnumerable<EntityInfo> entityInfos = await _commonService.GetEntityInfos();
        return entityInfos.Where(u => dbTableNames.Contains(config.EnableUnderLine ? UtilMethods.ToUnderLine(u.DbTableName) : u.DbTableName.ToLower())).Select(u => new TableOutput()
        {
            ConfigId = configId,
            EntityName = u.EntityName,
            TableName = config.EnableUnderLine ? UtilMethods.ToUnderLine(u.DbTableName) : u.DbTableName,
            TableComment = u.TableDescription
        }).ToList();
    }

    /// <summary>
    /// 根据表名获取列集合
    /// </summary>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "ColumnListByTableName")]
    [DisplayName("根据表名获取列集合")]
    public List<ColumnOuput> GetColumnListByTableName([Required] string tableName, string configId = SqlSugarConst.ConfigId)
    {
        // 切库---多库代码生成用
        var provider = _db.AsTenant().GetConnectionScope(configId);

        // 获取实体类型属性
        var entityType = provider.DbMaintenance.GetTableInfoList(false).FirstOrDefault(u => u.Name == tableName);
        if (entityType == null) return null;
        var entityBasePropertyNames = _codeGenOptions.EntityBaseColumn[nameof(EntityTenant)];
        // 按原始类型的顺序获取所有实体类型属性（不包含导航属性，会返回null）
        return provider.DbMaintenance.GetColumnInfosByTableName(entityType.Name).Select(u => new ColumnOuput
        {
            ColumnName = CodeGenUtil.CamelColumnName(u.DbColumnName, entityBasePropertyNames),
            ColumnKey = u.IsPrimarykey.ToString(),
            DataType = u.DataType.ToString(),
            NetType = CodeGenUtil.ConvertDataType(u),
            ColumnComment = u.ColumnDescription
        }).ToList();
    }

    /// <summary>
    /// 获取数据表列（实体属性）集合
    /// </summary>
    /// <returns></returns>
    private List<ColumnOuput> GetColumnList([FromQuery] AddCodeGenInput input)
    {
        var entityType = _commonService.GetEntityInfos().Result.FirstOrDefault(m => m.EntityName == input.TableName);
        if (entityType == null)
            return null;

        // 切库---多库代码生成用
        var provider = _db.AsTenant().GetConnectionScope(!string.IsNullOrEmpty(input.ConfigId) ? input.ConfigId : SqlSugarConst.ConfigId);

        var config = App.GetOptions<DbConnectionOptions>().ConnectionConfigs.FirstOrDefault(u => u.ConfigId == input.ConfigId);
        var dbTableName = config.EnableUnderLine ? UtilMethods.ToUnderLine(entityType.DbTableName) : entityType.DbTableName;
        var entityBasePropertyNames = _codeGenOptions.EntityBaseColumn[nameof(EntityTenant)];
        return provider.DbMaintenance.GetColumnInfosByTableName(dbTableName, false).Select(u => new ColumnOuput
        {
            //转下划线后的列名 需要转回来
            ColumnName = config.EnableUnderLine ? CodeGenUtil.CamelColumnName(u.DbColumnName, entityBasePropertyNames) : u.DbColumnName,
            ColumnKey = u.IsPrimarykey.ToString(),
            NetType = CodeGenUtil.ConvertDataType(u),
            DataType = CodeGenUtil.ConvertDataType(u),
            ColumnComment = string.IsNullOrWhiteSpace(u.ColumnDescription) ? u.DbColumnName : u.ColumnDescription
        }).ToList();
    }

    /// <summary>
    /// 代码生成到本地
    /// </summary>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "RunLocal")]
    [DisplayName("代码生成到本地")]
    public async Task RunLocal(SysCodeGen input)
    {
        // 先删除该表已生成的菜单列表
        var templatePathList = GetTemplatePathList();
        var targetPathList = GetTargetPathList(input);

        var tableFieldList = await _codeGenConfigService.GetList(new CodeGenConfig() { CodeGenId = input.Id }); // 字段集合

        var queryWhetherList = tableFieldList.Where(u => u.QueryWhether == YesNoEnum.Y.ToString()).ToList(); // 前端查询集合
        var joinTableList = tableFieldList.Where(u => u.EffectType == "Upload" || u.EffectType == "fk").ToList();// 需要连表查询的字段
        (string joinTableNames, string lowerJoinTableNames) = GetJoinTableStr(joinTableList);// 获取连表的实体名和别名

        var data = new CustomViewEngine(_db)
        {
            ConfigId = input.ConfigId,
            AuthorName = input.AuthorName,
            BusName = input.BusName,
            NameSpace = input.NameSpace,
            ClassName = input.TableName,
            ProjectLastName = input.NameSpace.Split('.').Last(),
            QueryWhetherList = queryWhetherList,
            TableField = tableFieldList,
            IsJoinTable = joinTableList.Count > 0,
            IsUpload = joinTableList.Where(u => u.EffectType == "Upload").Any(),
        };

        for (var i = 0; i < templatePathList.Count; i++)
        {
            var tContent = File.ReadAllText(templatePathList[i]);

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

    /// <summary>
    /// 增加菜单
    /// </summary>
    /// <param name="className"></param>
    /// <param name="busName"></param>
    /// <param name="pid"></param>
    /// <returns></returns>
    private async Task AddMenu(string className, string busName, long pid)
    {
        string pPath = null;//父路径
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
            {   //如果之前存在那么就删除本级和下级
                var list = await _db.Queryable<SysMenu>().Where(e => e.Title == menuType0.Title && e.Type == menuType0.Type).ToListAsync();
                if (list.Count > 0)
                {
                    var listIds = list.Select(f => f.Id).ToList();
                    var _ChildlistIds = new List<long>();
                    foreach (var item in listIds)
                    {
                        var _Childlist = await _db.Queryable<SysMenu>().ToChildListAsync(u => u.Pid, item);
                        _ChildlistIds.AddRange(_Childlist.Select(f => f.Id).ToList());
                    }
                    listIds.AddRange(_ChildlistIds);
                    await _db.Deleteable<SysMenu>().Where(e => listIds.Contains(e.Id)).ExecuteCommandAsync();
                    await _db.Deleteable<SysRoleMenu>().Where(e => listIds.Contains(e.MenuId)).ExecuteCommandAsync();
                }
            }
            pid = (await _db.Insertable(menuType0).ExecuteReturnEntityAsync()).Id;
        }
        // 由于后续菜单会有修改, 需要判断下 pid 是否存在, 不存在报错
        else
        {
            var pMenu = await _db.Queryable<SysMenu>().FirstAsync(e => e.Id == pid);
            if (pMenu == null)
                throw Oops.Oh(ErrorCodeEnum.D1505);
            else
            {
                pPath = pMenu.Path;
            }
        }

        // 菜单
        var menuType1 = new SysMenu
        {
            Pid = pid,
            Title = busName + "管理",
            Name = className + "Management",
            Type = MenuTypeEnum.Menu,
            Path = pPath + "/" + className.ToLower(),
            Component = "/main/" + className[..1].ToLower() + className[1..] + "/index",
        };
        {   // 如果之前存在那么就删除本级和下级
            var list = await _db.Queryable<SysMenu>().Where(e => e.Title == menuType1.Title && e.Type == menuType1.Type).ToListAsync();
            if (list.Count > 0)
            {
                var listIds = list.Select(f => f.Id).ToList();
                var _ChildlistIds = new List<long>();
                foreach (var item in listIds)
                {
                    var _Childlist = await _db.Queryable<SysMenu>().ToChildListAsync(u => u.Pid, item);
                    _ChildlistIds.AddRange(_Childlist.Select(f => f.Id).ToList());
                }
                listIds.AddRange(_ChildlistIds);
                await _db.Deleteable<SysMenu>().Where(e => listIds.Contains(e.Id)).ExecuteCommandAsync();
                await _db.Deleteable<SysRoleMenu>().Where(e => listIds.Contains(e.MenuId)).ExecuteCommandAsync();
            }
        }
        var pid1 = (await _db.Insertable(menuType1).ExecuteReturnEntityAsync()).Id;

        // 按钮-page
        var menuType2 = new SysMenu
        {
            Pid = pid1,
            Title = busName + "查询",
            Type = MenuTypeEnum.Btn,
            Permission = className[..1].ToLower() + className[1..] + ":page",
        };

        // 按钮-detail
        var menuType2_1 = new SysMenu
        {
            Pid = pid1,
            Title = busName + "详情",
            Type = MenuTypeEnum.Btn,
            Permission = className[..1].ToLower() + className[1..] + ":detail",
        };

        // 按钮-add
        var menuType2_2 = new SysMenu
        {
            Pid = pid1,
            Title = busName + "增加",
            Type = MenuTypeEnum.Btn,
            Permission = className[..1].ToLower() + className[1..] + ":add",
        };

        // 按钮-delete
        var menuType2_3 = new SysMenu
        {
            Pid = pid1,
            Title = busName + "删除",
            Type = MenuTypeEnum.Btn,
            Permission = className[..1].ToLower() + className[1..] + ":delete",
        };

        // 按钮-edit
        var menuType2_4 = new SysMenu
        {
            Pid = pid1,
            Title = busName + "编辑",
            Type = MenuTypeEnum.Btn,
            Permission = className[..1].ToLower() + className[1..] + ":edit",
        };

        var menuList = new List<SysMenu>() { menuType2, menuType2_1, menuType2_2, menuType2_3, menuType2_4 };
        await _db.Insertable(menuList).ExecuteCommandAsync();
    }

    /// <summary>
    /// 获取模板文件路径集合
    /// </summary>
    /// <returns></returns>
    private static List<string> GetTemplatePathList()
    {
        var templatePath = Path.Combine(App.WebHostEnvironment.WebRootPath, "Template");
        return new List<string>()
        {
            Path.Combine(templatePath , "Service.cs.vm"),
            Path.Combine(templatePath , "Input.cs.vm"),
            Path.Combine(templatePath , "Output.cs.vm"),
            Path.Combine(templatePath , "Dto.cs.vm"),
            Path.Combine(templatePath , "index.vue.vm"),
            Path.Combine(templatePath , "editDialog.vue.vm"),
            Path.Combine(templatePath , "manage.js.vm"),
        };
    }

    /// <summary>
    /// 设置生成文件路径
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private List<string> GetTargetPathList(SysCodeGen input)
    {
        var backendPath = Path.Combine(new DirectoryInfo(App.WebHostEnvironment.ContentRootPath).Parent.FullName, _codeGenOptions.BackendApplicationNamespace, "Service", input.TableName);
        var servicePath = Path.Combine(backendPath, input.TableName + "Service.cs");
        var inputPath = Path.Combine(backendPath, "Dto", input.TableName + "Input.cs");
        var outputPath = Path.Combine(backendPath, "Dto", input.TableName + "Output.cs");
        var viewPath = Path.Combine(backendPath, "Dto", input.TableName + "Dto.cs");
        var frontendPath = Path.Combine(new DirectoryInfo(App.WebHostEnvironment.ContentRootPath).Parent.Parent.FullName, _codeGenOptions.FrontRootPath, "src", "views", "main");
        var indexPath = Path.Combine(frontendPath, input.TableName[..1].ToLower() + input.TableName[1..], "index.vue");//
        var formModalPath = Path.Combine(frontendPath, input.TableName[..1].ToLower() + input.TableName[1..], "component", "editDialog.vue");
        var apiJsPath = Path.Combine(new DirectoryInfo(App.WebHostEnvironment.ContentRootPath).Parent.Parent.FullName, _codeGenOptions.FrontRootPath, "src", "api", "main", input.TableName[..1].ToLower() + input.TableName[1..] + ".ts");

        return new List<string>()
        {
            servicePath,
            inputPath,
            outputPath,
            viewPath,
            indexPath,
            formModalPath,
            apiJsPath
        };
    }
}