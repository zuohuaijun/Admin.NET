// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using System.IO.Compression;

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
    private readonly CodeGenOptions _codeGenOptions;

    public SysCodeGenService(ISqlSugarClient db,
        SysCodeGenConfigService codeGenConfigService,
        IViewEngine viewEngine,
        IOptions<CodeGenOptions> codeGenOptions)
    {
        _db = db;
        _codeGenConfigService = codeGenConfigService;
        _viewEngine = viewEngine;
        _codeGenOptions = codeGenOptions.Value;
    }

    /// <summary>
    /// 获取代码生成分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取代码生成分页列表")]
    public async Task<SqlSugarPagedList<SysCodeGen>> Page(CodeGenInput input)
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
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
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
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
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
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
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
    [DisplayName("获取代码生成详情")]
    public async Task<SysCodeGen> GetDetail([FromQuery] QueryCodeGenInput input)
    {
        return await _db.Queryable<SysCodeGen>().SingleAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 获取数据库库集合
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取数据库库集合")]
    public async Task<List<DatabaseOutput>> GetDatabaseList()
    {
        var dbConfigs = App.GetOptions<DbConnectionOptions>().ConnectionConfigs;
        return await Task.FromResult(dbConfigs.Adapt<List<DatabaseOutput>>());
    }

    /// <summary>
    /// 获取数据库表(实体)集合
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取数据库表(实体)集合")]
    public async Task<List<TableOutput>> GetTableList(string configId = SqlSugarConst.MainConfigId)
    {
        var provider = _db.AsTenant().GetConnectionScope(configId);
        var dbTableInfos = provider.DbMaintenance.GetTableInfoList(false); // 不能走缓存,否则切库不起作用

        var config = App.GetOptions<DbConnectionOptions>().ConnectionConfigs.FirstOrDefault(u => configId.Equals(u.ConfigId));

        var dbTableNames = dbTableInfos.Select(u => u.Name.ToLower()).ToList();
        IEnumerable<EntityInfo> entityInfos = await GetEntityInfos();

        var tableOutputList = new List<TableOutput>();
        foreach (var item in entityInfos)
        {
            var table = dbTableInfos.FirstOrDefault(u => u.Name.ToLower() == (config.DbSettings.EnableUnderLine ? UtilMethods.ToUnderLine(item.DbTableName) : item.DbTableName).ToLower());
            if (table == null) continue;
            tableOutputList.Add(new TableOutput
            {
                ConfigId = configId,
                EntityName = item.EntityName,
                TableName = table.Name,
                TableComment = item.TableDescription
            });
        }
        return tableOutputList;
    }

    /// <summary>
    /// 根据表名获取列集合
    /// </summary>
    /// <returns></returns>
    [DisplayName("根据表名获取列集合")]
    public List<ColumnOuput> GetColumnListByTableName([Required] string tableName, string configId = SqlSugarConst.MainConfigId)
    {
        // 切库---多库代码生成用
        var provider = _db.AsTenant().GetConnectionScope(configId);

        var config = App.GetOptions<DbConnectionOptions>().ConnectionConfigs.FirstOrDefault(u => u.ConfigId.ToString() == configId);
        // 获取实体类型属性
        var entityType = provider.DbMaintenance.GetTableInfoList(false).FirstOrDefault(u => u.Name == tableName);
        if (entityType == null) return null;
        var entityBasePropertyNames = _codeGenOptions.EntityBaseColumn[nameof(EntityTenant)];
        // 按原始类型的顺序获取所有实体类型属性（不包含导航属性，会返回null）
        return provider.DbMaintenance.GetColumnInfosByTableName(entityType.Name).Select(u => new ColumnOuput
        {
            ColumnName = config.DbSettings.EnableUnderLine ? CodeGenUtil.CamelColumnName(u.DbColumnName, entityBasePropertyNames) : u.DbColumnName,
            ColumnKey = u.IsPrimarykey.ToString(),
            DataType = u.DataType.ToString(),
            NetType = CodeGenUtil.ConvertDataType(u, provider.CurrentConnectionConfig.DbType),
            ColumnComment = u.ColumnDescription
        }).ToList();
    }

    /// <summary>
    /// 获取数据表列（实体属性）集合
    /// </summary>
    /// <returns></returns>
    private List<ColumnOuput> GetColumnList([FromQuery] AddCodeGenInput input)
    {
        var entityType = GetEntityInfos().GetAwaiter().GetResult().FirstOrDefault(u => u.EntityName == input.TableName);
        if (entityType == null)
            return null;

        // 切库---多库代码生成用
        var provider = _db.AsTenant().GetConnectionScope(!string.IsNullOrEmpty(input.ConfigId) ? input.ConfigId : SqlSugarConst.MainConfigId);

        var config = App.GetOptions<DbConnectionOptions>().ConnectionConfigs.FirstOrDefault(u => u.ConfigId.ToString() == input.ConfigId);
        var dbTableName = config.DbSettings.EnableUnderLine ? UtilMethods.ToUnderLine(entityType.DbTableName) : entityType.DbTableName;
        var entityBasePropertyNames = _codeGenOptions.EntityBaseColumn[nameof(EntityTenant)];
        var columnInfos = provider.DbMaintenance.GetColumnInfosByTableName(dbTableName, false);
        var result = columnInfos.Select(u => new ColumnOuput
        {
            // 转下划线后的列名需要再转回来（暂时不转）
            //ColumnName = config.DbSettings.EnableUnderLine ? CodeGenUtil.CamelColumnName(u.DbColumnName, entityBasePropertyNames) : u.DbColumnName,
            ColumnName = u.DbColumnName,
            ColumnLength = u.Length,
            IsPrimarykey = u.IsPrimarykey,
            IsNullable = u.IsNullable,
            ColumnKey = u.IsPrimarykey.ToString(),
            NetType = CodeGenUtil.ConvertDataType(u, provider.CurrentConnectionConfig.DbType),
            DataType = CodeGenUtil.ConvertDataType(u, provider.CurrentConnectionConfig.DbType),
            ColumnComment = string.IsNullOrWhiteSpace(u.ColumnDescription) ? u.DbColumnName : u.ColumnDescription
        }).ToList();

        // 获取实体的属性信息，赋值给PropertyName属性(CodeFirst模式应以PropertyName为实际使用名称)
        var entityProperties = entityType.Type.GetProperties();

        for (int i = result.Count - 1; i >= 0; i--)
        {
            var columnOutput = result[i];
            // 先找自定义字段名的
            var propertyInfo = entityProperties.FirstOrDefault(p => (p.GetCustomAttribute<SugarColumn>()?.ColumnName ?? "").ToLower() == columnOutput.ColumnName.ToLower());
            // 如果找不到就再找自动生成字段名的(并且过滤掉没有SugarColumn的属性)
            if (propertyInfo == null)
                propertyInfo = entityProperties.FirstOrDefault(p => p.GetCustomAttribute<SugarColumn>() != null && p.Name == (config.DbSettings.EnableUnderLine ? CodeGenUtil.CamelColumnName(columnOutput.ColumnName, entityBasePropertyNames) : columnOutput.ColumnName));
            if (propertyInfo != null)
                columnOutput.PropertyName = propertyInfo.Name;
            else
                result.RemoveAt(i); //移除没有定义此属性的字段
        }
        return result;
    }

    /// <summary>
    /// 获取库表信息
    /// </summary>
    /// <returns></returns>
    private async Task<IEnumerable<EntityInfo>> GetEntityInfos()
    {
        var entityInfos = new List<EntityInfo>();

        var type = typeof(SugarTable);
        var types = new List<Type>();
        if (_codeGenOptions.EntityAssemblyNames != null)
        {
            foreach (var assemblyName in _codeGenOptions.EntityAssemblyNames)
            {
                Assembly asm = Assembly.Load(assemblyName);
                types.AddRange(asm.GetExportedTypes().ToList());
            }
        }
        bool IsMyAttribute(Attribute[] o)
        {
            foreach (Attribute a in o)
            {
                if (a.GetType() == type)
                    return true;
            }
            return false;
        }
        Type[] cosType = types.Where(o =>
        {
            return IsMyAttribute(Attribute.GetCustomAttributes(o, true));
        }
        ).ToArray();

        foreach (var c in cosType)
        {
            var sugarAttribute = c.GetCustomAttributes(type, true)?.FirstOrDefault();

            var des = c.GetCustomAttributes(typeof(DescriptionAttribute), true);
            var description = "";
            if (des.Length > 0)
            {
                description = ((DescriptionAttribute)des[0]).Description;
            }
            entityInfos.Add(new EntityInfo()
            {
                EntityName = c.Name,
                DbTableName = sugarAttribute == null ? c.Name : ((SugarTable)sugarAttribute).TableName,
                TableDescription = description,
                Type = c
            });
        }
        return await Task.FromResult(entityInfos);
    }

    /// <summary>
    /// 获取程序保存位置
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取程序保存位置")]
    public List<string> GetApplicationNamespaces()
    {
        return _codeGenOptions.BackendApplicationNamespaces;
    }

    /// <summary>
    /// 代码生成到本地
    /// </summary>
    /// <returns></returns>
    [DisplayName("代码生成到本地")]
    public async Task<dynamic> RunLocal(SysCodeGen input)
    {
        if (string.IsNullOrEmpty(input.GenerateType))
            input.GenerateType = "200";

        // 先删除该表已生成的菜单列表
        var templatePathList = GetTemplatePathList(input);
        List<string> targetPathList;
        var zipPath = Path.Combine(App.WebHostEnvironment.WebRootPath, "CodeGen", input.TableName);
        if (input.GenerateType.StartsWith('1'))
        {
            targetPathList = GetZipPathList(input);
            if (Directory.Exists(zipPath))
                Directory.Delete(zipPath, true);
        }
        else
            targetPathList = GetTargetPathList(input);

        var tableFieldList = await _codeGenConfigService.GetList(new CodeGenConfig() { CodeGenId = input.Id }); // 字段集合
        var queryWhetherList = tableFieldList.Where(u => u.QueryWhether == YesNoEnum.Y.ToString()).ToList(); // 前端查询集合
        var joinTableList = tableFieldList.Where(u => u.EffectType == "Upload" || u.EffectType == "fk" || u.EffectType == "ApiTreeSelect").ToList(); // 需要连表查询的字段
        (string joinTableNames, string lowerJoinTableNames) = GetJoinTableStr(joinTableList); // 获取连表的实体名和别名

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
            var tResult = await _viewEngine.RunCompileFromCachedAsync(tContent, data, builderAction: builder =>
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

        await AddMenu(input.TableName, input.BusName, input.MenuPid, tableFieldList);
        // 非ZIP压缩返回空
        if (!input.GenerateType.StartsWith('1'))
            return null;
        else
        {
            string downloadPath = zipPath + ".zip";
            // 判断是否存在同名称文件
            if (File.Exists(downloadPath))
                File.Delete(downloadPath);
            ZipFile.CreateFromDirectory(zipPath, downloadPath);
            return new { url = $"{CommonUtil.GetLocalhost()}/CodeGen/{input.TableName}.zip" };
        }
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
        string str = ""; // <Order, OrderItem, Custom>
        string lowerStr = ""; // (o, i, c)
        foreach (var item in uploads)
        {
            lowerStr += "sysFile_FK_" + item.LowerPropertyName + ",";
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
    /// <param name="tableFieldList"></param>
    /// <returns></returns>
    private async Task AddMenu(string className, string busName, long pid, List<CodeGenConfig> tableFieldList)
    {
        var pPath = string.Empty;
        // 若 pid=0 为顶级则创建菜单目录
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
            // 若先前存在则删除本级和下级
            var menuList0 = await _db.Queryable<SysMenu>().Where(u => u.Title == menuType0.Title && u.Type == menuType0.Type).ToListAsync();
            if (menuList0.Count > 0)
            {
                var listIds = menuList0.Select(u => u.Id).ToList();
                var childlistIds = new List<long>();
                foreach (var item in listIds)
                {
                    var childlist = await _db.Queryable<SysMenu>().ToChildListAsync(u => u.Pid, item);
                    childlistIds.AddRange(childlist.Select(u => u.Id).ToList());
                }
                listIds.AddRange(childlistIds);
                await _db.Deleteable<SysMenu>().Where(u => listIds.Contains(u.Id)).ExecuteCommandAsync();
                await _db.Deleteable<SysRoleMenu>().Where(u => listIds.Contains(u.MenuId)).ExecuteCommandAsync();
            }
            pid = (await _db.Insertable(menuType0).ExecuteReturnEntityAsync()).Id;
        }
        else
        {
            var pMenu = await _db.Queryable<SysMenu>().FirstAsync(u => u.Id == pid) ?? throw Oops.Oh(ErrorCodeEnum.D1505);
            pPath = pMenu.Path;
        }

        // 菜单
        var menuType1 = new SysMenu
        {
            Pid = pid,
            Title = busName + "管理",
            Name = className[..1].ToLower() + className[1..],
            Type = MenuTypeEnum.Menu,
            Path = pPath + "/" + className.ToLower(),
            Component = "/main/" + className[..1].ToLower() + className[1..] + "/index",
        };
        // 若先前存在则删除本级和下级
        var menuList1 = await _db.Queryable<SysMenu>().Where(u => u.Title == menuType1.Title && u.Type == menuType1.Type).ToListAsync();
        if (menuList1.Count > 0)
        {
            var listIds = menuList1.Select(u => u.Id).ToList();
            var childlistIds = new List<long>();
            foreach (var item in listIds)
            {
                var childlist = await _db.Queryable<SysMenu>().ToChildListAsync(u => u.Pid, item);
                childlistIds.AddRange(childlist.Select(u => u.Id).ToList());
            }
            listIds.AddRange(childlistIds);
            await _db.Deleteable<SysMenu>().Where(u => listIds.Contains(u.Id)).ExecuteCommandAsync();
            await _db.Deleteable<SysRoleMenu>().Where(u => listIds.Contains(u.MenuId)).ExecuteCommandAsync();
        }

        var pid1 = (await _db.Insertable(menuType1).ExecuteReturnEntityAsync()).Id;
        int menuOrder = 100;
        // 按钮-page
        var menuType2 = new SysMenu
        {
            Pid = pid1,
            Title = "查询",
            Type = MenuTypeEnum.Btn,
            Permission = className[..1].ToLower() + className[1..] + ":page",
            OrderNo = menuOrder
        };
        menuOrder += 10;

        // 按钮-detail
        var menuType2_1 = new SysMenu
        {
            Pid = pid1,
            Title = "详情",
            Type = MenuTypeEnum.Btn,
            Permission = className[..1].ToLower() + className[1..] + ":detail",
            OrderNo = menuOrder
        };
        menuOrder += 10;

        // 按钮-add
        var menuType2_2 = new SysMenu
        {
            Pid = pid1,
            Title = "增加",
            Type = MenuTypeEnum.Btn,
            Permission = className[..1].ToLower() + className[1..] + ":add",
            OrderNo = menuOrder
        };
        menuOrder += 10;

        // 按钮-delete
        var menuType2_3 = new SysMenu
        {
            Pid = pid1,
            Title = "删除",
            Type = MenuTypeEnum.Btn,
            Permission = className[..1].ToLower() + className[1..] + ":delete",
            OrderNo = menuOrder
        };
        menuOrder += 10;

        // 按钮-edit
        var menuType2_4 = new SysMenu
        {
            Pid = pid1,
            Title = "编辑",
            Type = MenuTypeEnum.Btn,
            Permission = className[..1].ToLower() + className[1..] + ":edit",
            OrderNo = menuOrder
        };
        menuOrder += 10;

        var menuList = new List<SysMenu>() { menuType2, menuType2_1, menuType2_2, menuType2_3, menuType2_4 };
        // 加入fk、Upload、ApiTreeSelect 等接口的权限
        // 在生成表格时，有些字段只是查询时显示，不需要填写（WhetherAddUpdate），所以这些字段没必要生成相应接口
        var fkTableList = tableFieldList.Where(u => u.EffectType == "fk" && (u.WhetherAddUpdate == "Y" || u.QueryWhether == "Y")).ToList();
        foreach (var @column in fkTableList)
        {
            var menuType = new SysMenu
            {
                Pid = pid1,
                Title = "外键" + @column.ColumnName,
                Type = MenuTypeEnum.Btn,
                Permission = className[..1].ToLower() + className[1..] + ":" + column.FkEntityName + column.ColumnName + "Dropdown",
                OrderNo = menuOrder
            };
            menuOrder += 10;
            menuList.Add(menuType);
        }
        var treeSelectTableList = tableFieldList.Where(u => u.EffectType == "ApiTreeSelect").ToList();
        foreach (var @column in treeSelectTableList)
        {
            var menuType = new SysMenu
            {
                Pid = pid1,
                Title = "树型" + @column.ColumnName,
                Type = MenuTypeEnum.Btn,
                Permission = className[..1].ToLower() + className[1..] + ":" + column.FkEntityName + "Tree",
                OrderNo = menuOrder
            };
            menuOrder += 10;
            menuList.Add(menuType);
        }
        var uploadTableList = tableFieldList.Where(u => u.EffectType == "Upload").ToList();
        foreach (var @column in uploadTableList)
        {
            var menuType = new SysMenu
            {
                Pid = pid1,
                Title = "上传" + @column.ColumnName,
                Type = MenuTypeEnum.Btn,
                Permission = className[..1].ToLower() + className[1..] + ":Upload" + column.ColumnName,
                OrderNo = menuOrder
            };
            menuOrder += 10;
            menuList.Add(menuType);
        }
        await _db.Insertable(menuList).ExecuteCommandAsync();
    }

    /// <summary>
    /// 获取模板文件路径集合
    /// </summary>
    /// <returns></returns>
    private static List<string> GetTemplatePathList(SysCodeGen input)
    {
        var templatePath = Path.Combine(App.WebHostEnvironment.WebRootPath, "Template");
        if (input.GenerateType.Substring(1, 1).Contains('1'))
        {
            return new List<string>()
            {
                Path.Combine(templatePath , "index.vue.vm"),
                Path.Combine(templatePath , "editDialog.vue.vm"),
                Path.Combine(templatePath , "manage.js.vm"),
            };
        }
        else if (input.GenerateType.Substring(1, 1).Contains('2'))
        {
            return new List<string>()
            {
                Path.Combine(templatePath , "Service.cs.vm"),
                Path.Combine(templatePath , "Input.cs.vm"),
                Path.Combine(templatePath , "Output.cs.vm"),
                Path.Combine(templatePath , "Dto.cs.vm"),
            };
        }
        else
        {
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
        //var backendPath = Path.Combine(new DirectoryInfo(App.WebHostEnvironment.ContentRootPath).Parent.FullName, _codeGenOptions.BackendApplicationNamespace, "Service", input.TableName);
        var backendPath = Path.Combine(new DirectoryInfo(App.WebHostEnvironment.ContentRootPath).Parent.FullName, input.NameSpace, "Service", input.TableName);
        var servicePath = Path.Combine(backendPath, input.TableName + "Service.cs");
        var inputPath = Path.Combine(backendPath, "Dto", input.TableName + "Input.cs");
        var outputPath = Path.Combine(backendPath, "Dto", input.TableName + "Output.cs");
        var viewPath = Path.Combine(backendPath, "Dto", input.TableName + "Dto.cs");
        var frontendPath = Path.Combine(new DirectoryInfo(App.WebHostEnvironment.ContentRootPath).Parent.Parent.FullName, _codeGenOptions.FrontRootPath, "src", "views", "main");
        var indexPath = Path.Combine(frontendPath, input.TableName[..1].ToLower() + input.TableName[1..], "index.vue");//
        var formModalPath = Path.Combine(frontendPath, input.TableName[..1].ToLower() + input.TableName[1..], "component", "editDialog.vue");
        var apiJsPath = Path.Combine(new DirectoryInfo(App.WebHostEnvironment.ContentRootPath).Parent.Parent.FullName, _codeGenOptions.FrontRootPath, "src", "api", "main", input.TableName[..1].ToLower() + input.TableName[1..] + ".ts");

        if (input.GenerateType.Substring(1, 1).Contains('1'))
        {
            // 生成到本项目(前端)
            return new List<string>()
            {
                indexPath,
                formModalPath,
                apiJsPath
            };
        }
        else if (input.GenerateType.Substring(1, 1).Contains('2'))
        {
            // 生成到本项目(后端)
            return new List<string>()
            {
                servicePath,
                inputPath,
                outputPath,
                viewPath,
            };
        }
        else
        {
            // 前后端同时生成到本项目
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

    /// <summary>
    /// 设置生成文件路径
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private List<string> GetZipPathList(SysCodeGen input)
    {
        var zipPath = Path.Combine(App.WebHostEnvironment.WebRootPath, "CodeGen", input.TableName);

        //var backendPath = Path.Combine(zipPath, _codeGenOptions.BackendApplicationNamespace, "Service", input.TableName);
        var backendPath = Path.Combine(zipPath, input.NameSpace, "Service", input.TableName);
        var servicePath = Path.Combine(backendPath, input.TableName + "Service.cs");
        var inputPath = Path.Combine(backendPath, "Dto", input.TableName + "Input.cs");
        var outputPath = Path.Combine(backendPath, "Dto", input.TableName + "Output.cs");
        var viewPath = Path.Combine(backendPath, "Dto", input.TableName + "Dto.cs");
        var frontendPath = Path.Combine(zipPath, _codeGenOptions.FrontRootPath, "src", "views", "main");
        var indexPath = Path.Combine(frontendPath, input.TableName[..1].ToLower() + input.TableName[1..], "index.vue");
        var formModalPath = Path.Combine(frontendPath, input.TableName[..1].ToLower() + input.TableName[1..], "component", "editDialog.vue");
        var apiJsPath = Path.Combine(zipPath, _codeGenOptions.FrontRootPath, "src", "api", "main", input.TableName[..1].ToLower() + input.TableName[1..] + ".ts");
        if (input.GenerateType.StartsWith("11"))
        {
            return new List<string>()
            {
                indexPath,
                formModalPath,
                apiJsPath
            };
        }
        else if (input.GenerateType.StartsWith("12"))
        {
            return new List<string>()
            {
                servicePath,
                inputPath,
                outputPath,
                viewPath
            };
        }
        else
        {
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
}