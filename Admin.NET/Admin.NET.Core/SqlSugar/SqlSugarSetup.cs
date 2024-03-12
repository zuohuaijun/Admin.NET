// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core;

public static class SqlSugarSetup
{
    // 缓存所有仓储连接实例
    private static ConcurrentDictionary<Type, ISqlSugarClient> iClientAttrDict = new();

    /// <summary>
    /// SqlSugar 上下文初始化
    /// </summary>
    /// <param name="services"></param>
    public static void AddSqlSugar(this IServiceCollection services)
    {
        //// 注册雪花Id
        //var snowIdOpt = App.GetConfig<SnowIdOptions>("SnowId", true);
        //YitIdHelper.SetIdGenerator(snowIdOpt);

        // 注册雪花Id-支持分布式
        var snowIdOpt = App.GetConfig<SnowIdOptions>("SnowId", true);
        services.AddYitIdHelper(snowIdOpt);

        // 自定义 SqlSugar 雪花ID算法
        SnowFlakeSingle.WorkId = snowIdOpt.WorkerId;
        StaticConfig.CustomSnowFlakeFunc = () =>
        {
            return YitIdHelper.NextId();
        };

        var dbOptions = App.GetConfig<DbConnectionOptions>("DbConnection", true);
        dbOptions.ConnectionConfigs.ForEach(SetDbConfig);

        SqlSugarScope sqlSugar = new(dbOptions.ConnectionConfigs.Adapt<List<ConnectionConfig>>(), db =>
        {
            dbOptions.ConnectionConfigs.ForEach(config =>
            {
                var dbProvider = db.GetConnectionScope(config.ConfigId);
                SetDbAop(dbProvider, dbOptions.EnableConsoleSql);
                SetDbDiffLog(dbProvider, config);
            });
        });
        InitSqlSugarRepository(sqlSugar.AsTenant()); // 初始化仓储实例

        services.AddSingleton<ISqlSugarClient>(sqlSugar); // 单例注册
        services.AddScoped(typeof(SqlSugarRepository<>)); // 仓储注册
        services.AddUnitOfWork<SqlSugarUnitOfWork>(); // 事务与工作单元注册

        // 初始化数据库表结构及种子数据
        dbOptions.ConnectionConfigs.ForEach(config =>
        {
            InitDatabase(sqlSugar, config);
        });
    }

    /// <summary>
    /// 配置连接属性
    /// </summary>
    /// <param name="config"></param>
    public static void SetDbConfig(DbConnectionConfig config)
    {
        var configureExternalServices = new ConfigureExternalServices
        {
            EntityNameService = (type, entity) => // 处理表
            {
                entity.IsDisabledDelete = true; // 禁止删除非 sqlsugar 创建的列
                // 只处理贴了特性[SugarTable]表
                if (!type.GetCustomAttributes<SugarTable>().Any())
                    return;
                if (config.DbSettings.EnableUnderLine && !entity.DbTableName.Contains('_'))
                    entity.DbTableName = UtilMethods.ToUnderLine(entity.DbTableName); // 驼峰转下划线
            },
            EntityService = (type, column) => // 处理列
            {
                // 只处理贴了特性[SugarColumn]列
                if (!type.GetCustomAttributes<SugarColumn>().Any())
                    return;
                if (new NullabilityInfoContext().Create(type).WriteState is NullabilityState.Nullable)
                    column.IsNullable = true;
                if (config.DbSettings.EnableUnderLine && !column.IsIgnore && !column.DbColumnName.Contains('_'))
                    column.DbColumnName = UtilMethods.ToUnderLine(column.DbColumnName); // 驼峰转下划线
            },
            DataInfoCacheService = new SqlSugarCache(),
        };
        config.ConfigureExternalServices = configureExternalServices;
        config.InitKeyType = InitKeyType.Attribute;
        config.IsAutoCloseConnection = true;
        config.MoreSettings = new ConnMoreSettings
        {
            IsAutoRemoveDataCache = true, // 启用自动删除缓存，所有增删改会自动调用.RemoveDataCache()
            IsAutoDeleteQueryFilter = true, // 启用删除查询过滤器
            IsAutoUpdateQueryFilter = true, // 启用更新查询过滤器
            SqlServerCodeFirstNvarchar = true // 采用Nvarchar
        };
    }

    /// <summary>
    /// 配置Aop
    /// </summary>
    /// <param name="db"></param>
    /// <param name="enableConsoleSql"></param>
    public static void SetDbAop(SqlSugarScopeProvider db, bool enableConsoleSql)
    {
        // 设置超时时间
        db.Ado.CommandTimeOut = 30;

        // 打印SQL语句
        if (enableConsoleSql)
        {
            db.Aop.OnLogExecuting = (sql, pars) =>
            {
                var log = $"【{DateTime.Now}——执行SQL】\r\n{UtilMethods.GetNativeSql(sql, pars)}\r\n";
                var originColor = Console.ForegroundColor;
                if (sql.StartsWith("SELECT", StringComparison.OrdinalIgnoreCase))
                    Console.ForegroundColor = ConsoleColor.Green;
                if (sql.StartsWith("UPDATE", StringComparison.OrdinalIgnoreCase) || sql.StartsWith("INSERT", StringComparison.OrdinalIgnoreCase))
                    Console.ForegroundColor = ConsoleColor.Yellow;
                if (sql.StartsWith("DELETE", StringComparison.OrdinalIgnoreCase))
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(log);
                Console.ForegroundColor = originColor;
                App.PrintToMiniProfiler("SqlSugar", "Info", log);
            };
            db.Aop.OnError = ex =>
            {
                if (ex.Parametres == null) return;
                var log = $"【{DateTime.Now}——错误SQL】\r\n{UtilMethods.GetNativeSql(ex.Sql, (SugarParameter[])ex.Parametres)}\r\n";
                var originColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(log);
                Console.ForegroundColor = originColor;
                App.PrintToMiniProfiler("SqlSugar", "Error", log);
            };
            db.Aop.OnLogExecuted = (sql, pars) =>
            {
                // 执行时间超过5秒时
                if (db.Ado.SqlExecutionTime.TotalSeconds > 5)
                {
                    var fileName = db.Ado.SqlStackTrace.FirstFileName; // 文件名
                    var fileLine = db.Ado.SqlStackTrace.FirstLine; // 行号
                    var firstMethodName = db.Ado.SqlStackTrace.FirstMethodName; // 方法名
                    var log = $"【{DateTime.Now}——超时SQL】\r\n【所在文件名】：{fileName}\r\n【代码行数】：{fileLine}\r\n【方法名】：{firstMethodName}\r\n" + $"【SQL语句】：{UtilMethods.GetNativeSql(sql, pars)}";
                    var originColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine(log);
                    Console.ForegroundColor = originColor;
                    App.PrintToMiniProfiler("SqlSugar", "Slow", log);
                }
            };
        }
        // 数据审计
        db.Aop.DataExecuting = (oldValue, entityInfo) =>
        {
            if (entityInfo.OperationType == DataFilterType.InsertByObject)
            {
                // 主键(long类型)且没有值的---赋值雪花Id
                if (entityInfo.EntityColumnInfo.IsPrimarykey && entityInfo.EntityColumnInfo.PropertyInfo.PropertyType == typeof(long))
                {
                    var id = entityInfo.EntityColumnInfo.PropertyInfo.GetValue(entityInfo.EntityValue);
                    if (id == null || (long)id == 0)
                        entityInfo.SetValue(YitIdHelper.NextId());
                }
                if (entityInfo.PropertyName == nameof(EntityBase.CreateTime))
                    entityInfo.SetValue(DateTime.Now);
                if (App.User != null)
                {
                    if (entityInfo.PropertyName == nameof(EntityTenantId.TenantId))
                    {
                        var tenantId = ((dynamic)entityInfo.EntityValue).TenantId;
                        if (tenantId == null || tenantId == 0)
                            entityInfo.SetValue(App.User.FindFirst(ClaimConst.TenantId)?.Value);
                    }
                    else if (entityInfo.PropertyName == nameof(EntityBase.CreateUserId))
                    {
                        var createUserId = ((dynamic)entityInfo.EntityValue).CreateUserId;
                        if (createUserId == 0 || createUserId == null)
                            entityInfo.SetValue(App.User.FindFirst(ClaimConst.UserId)?.Value);
                    }
                    else if (entityInfo.PropertyName == nameof(EntityBase.CreateUserName))
                    {
                        var createUserName = ((dynamic)entityInfo.EntityValue).CreateUserName;
                        if (string.IsNullOrEmpty(createUserName))
                            entityInfo.SetValue(App.User.FindFirst(ClaimConst.RealName)?.Value);
                    }
                    else if (entityInfo.PropertyName == nameof(EntityBaseData.CreateOrgId))
                    {
                        var createOrgId = ((dynamic)entityInfo.EntityValue).CreateOrgId;
                        if (createOrgId == 0 || createOrgId == null)
                            entityInfo.SetValue(App.User.FindFirst(ClaimConst.OrgId)?.Value);
                    }
                    else if (entityInfo.PropertyName == nameof(EntityBaseData.CreateOrgName))
                    {
                        var createOrgName = ((dynamic)entityInfo.EntityValue).CreateOrgName;
                        if (string.IsNullOrEmpty(createOrgName))
                            entityInfo.SetValue(App.User.FindFirst(ClaimConst.OrgName)?.Value);
                    }
                }
            }
            if (entityInfo.OperationType == DataFilterType.UpdateByObject)
            {
                if (entityInfo.PropertyName == nameof(EntityBase.UpdateTime))
                    entityInfo.SetValue(DateTime.Now);
                else if (entityInfo.PropertyName == nameof(EntityBase.UpdateUserId))
                    entityInfo.SetValue(App.User?.FindFirst(ClaimConst.UserId)?.Value);
                else if (entityInfo.PropertyName == nameof(EntityBase.UpdateUserName))
                    entityInfo.SetValue(App.User?.FindFirst(ClaimConst.RealName)?.Value);
            }
        };

        // 超管排除其他过滤器
        if (App.User?.FindFirst(ClaimConst.AccountType)?.Value == ((int)AccountTypeEnum.SuperAdmin).ToString())
            return;

        // 配置假删除过滤器
        db.QueryFilter.AddTableFilter<IDeletedFilter>(u => u.IsDelete == false);

        // 配置租户过滤器
        var tenantId = App.User?.FindFirst(ClaimConst.TenantId)?.Value;
        if (!string.IsNullOrWhiteSpace(tenantId))
            db.QueryFilter.AddTableFilter<ITenantIdFilter>(u => u.TenantId == long.Parse(tenantId));

        // 配置用户机构（数据范围）过滤器
        SqlSugarFilter.SetOrgEntityFilter(db);

        // 配置自定义过滤器
        SqlSugarFilter.SetCustomEntityFilter(db);
    }

    /// <summary>
    /// 开启库表差异化日志
    /// </summary>
    /// <param name="db"></param>
    /// <param name="config"></param>
    private static void SetDbDiffLog(SqlSugarScopeProvider db, DbConnectionConfig config)
    {
        if (!config.DbSettings.EnableDiffLog) return;

        db.Aop.OnDiffLogEvent = async u =>
        {
            var logDiff = new SysLogDiff
            {
                // 操作后记录（字段描述、列名、值、表名、表描述）
                AfterData = JSON.Serialize(u.AfterData),
                // 操作前记录（字段描述、列名、值、表名、表描述）
                BeforeData = JSON.Serialize(u.BeforeData),
                // 传进来的对象
                BusinessData = JSON.Serialize(u.BusinessData),
                // 枚举（insert、update、delete）
                DiffType = u.DiffType.ToString(),
                Sql = UtilMethods.GetNativeSql(u.Sql, u.Parameters),
                Parameters = JSON.Serialize(u.Parameters),
                Elapsed = u.Time == null ? 0 : (long)u.Time.Value.TotalMilliseconds
            };
            await db.CopyNew().Insertable(logDiff).ExecuteCommandAsync();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(DateTime.Now + $"\r\n*****开始差异日志*****\r\n{Environment.NewLine}{JSON.Serialize(logDiff)}{Environment.NewLine}*****结束差异日志*****\r\n");
        };
    }

    /// <summary>
    /// 初始化数据库
    /// </summary>
    /// <param name="db"></param>
    /// <param name="config"></param>
    private static void InitDatabase(SqlSugarScope db, DbConnectionConfig config)
    {
        SqlSugarScopeProvider dbProvider = db.GetConnectionScope(config.ConfigId);

        // 初始化/创建数据库
        if (config.DbSettings.EnableInitDb)
        {
            if (config.DbType != SqlSugar.DbType.Oracle)
                dbProvider.DbMaintenance.CreateDatabase();
        }

        // 初始化表结构
        if (config.TableSettings.EnableInitTable)
        {
            var entityTypes = App.EffectiveTypes.Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass && u.IsDefined(typeof(SugarTable), false))
                .WhereIF(config.TableSettings.EnableIncreTable, u => u.IsDefined(typeof(IncreTableAttribute), false)).ToList();

            if (config.ConfigId.ToString() == SqlSugarConst.MainConfigId) // 默认库（有系统表特性、没有日志表和租户表特性）
                entityTypes = entityTypes.Where(u => u.GetCustomAttributes<SysTableAttribute>().Any() || (!u.GetCustomAttributes<LogTableAttribute>().Any() && !u.GetCustomAttributes<TenantAttribute>().Any())).ToList();
            else if (config.ConfigId.ToString() == SqlSugarConst.LogConfigId) // 日志库
                entityTypes = entityTypes.Where(u => u.GetCustomAttributes<LogTableAttribute>().Any()).ToList();
            else
                entityTypes = entityTypes.Where(u => u.GetCustomAttribute<TenantAttribute>()?.configId.ToString() == config.ConfigId.ToString()).ToList(); // 自定义的库

            foreach (var entityType in entityTypes)
            {
                if (entityType.GetCustomAttribute<SplitTableAttribute>() == null)
                    dbProvider.CodeFirst.InitTables(entityType);
                else
                    dbProvider.CodeFirst.SplitTables().InitTables(entityType);
            }
        }

        // 初始化种子数据
        if (config.SeedSettings.EnableInitSeed)
        {
            var seedDataTypes = App.EffectiveTypes.Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass && u.GetInterfaces().Any(i => i.HasImplementedRawGeneric(typeof(ISqlSugarEntitySeedData<>))))
                .WhereIF(config.SeedSettings.EnableIncreSeed, u => u.IsDefined(typeof(IncreSeedAttribute), false)).ToList();

            foreach (var seedType in seedDataTypes)
            {
                var entityType = seedType.GetInterfaces().First().GetGenericArguments().First();
                if (config.ConfigId.ToString() == SqlSugarConst.MainConfigId) // 默认库（有系统表特性、没有日志表和租户表特性）
                {
                    if (entityType.GetCustomAttribute<SysTableAttribute>() == null && (entityType.GetCustomAttribute<LogTableAttribute>() != null || entityType.GetCustomAttribute<TenantAttribute>() != null))
                        continue;
                }
                else if (config.ConfigId.ToString() == SqlSugarConst.LogConfigId) // 日志库
                {
                    if (entityType.GetCustomAttribute<LogTableAttribute>() == null)
                        continue;
                }
                else
                {
                    var att = entityType.GetCustomAttribute<TenantAttribute>(); // 自定义的库
                    if (att == null || att.configId.ToString() != config.ConfigId.ToString()) continue;
                }

                var instance = Activator.CreateInstance(seedType);
                var hasDataMethod = seedType.GetMethod("HasData");
                var seedData = ((IEnumerable)hasDataMethod?.Invoke(instance, null))?.Cast<object>();
                if (seedData == null) continue;

                var entityInfo = dbProvider.EntityMaintenance.GetEntityInfo(entityType);
                if (entityInfo.Columns.Any(u => u.IsPrimarykey))
                {
                    // 按主键进行批量增加和更新
                    var storage = dbProvider.StorageableByObject(seedData.ToList()).ToStorage();
                    storage.AsInsertable.ExecuteCommand();
                    storage.AsUpdateable.ExecuteCommand();
                }
                else
                {
                    // 无主键则只进行插入
                    if (!dbProvider.Queryable(entityInfo.DbTableName, entityInfo.DbTableName).Any())
                        dbProvider.InsertableByObject(seedData.ToList()).ExecuteCommand();
                }
            }
        }
    }

    /// <summary>
    /// 初始化租户业务数据库
    /// </summary>
    /// <param name="iTenant"></param>
    /// <param name="config"></param>
    public static void InitTenantDatabase(ITenant iTenant, DbConnectionConfig config)
    {
        SetDbConfig(config);

        if (!iTenant.IsAnyConnection(config.ConfigId.ToString()))
            iTenant.AddConnection(config);
        var db = iTenant.GetConnectionScope(config.ConfigId.ToString());
        db.DbMaintenance.CreateDatabase();

        // 获取所有业务表-初始化租户库表结构（排除系统表、日志表、特定库表）
        var entityTypes = App.EffectiveTypes.Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass && u.IsDefined(typeof(SugarTable), false) &&
            !u.IsDefined(typeof(SysTableAttribute), false) && !u.IsDefined(typeof(LogTableAttribute), false) && !u.IsDefined(typeof(TenantAttribute), false)).ToList();
        if (!entityTypes.Any()) return;

        foreach (var entityType in entityTypes)
        {
            var splitTable = entityType.GetCustomAttribute<SplitTableAttribute>();
            if (splitTable == null)
                db.CodeFirst.InitTables(entityType);
            else
                db.CodeFirst.SplitTables().InitTables(entityType);
        }
    }

    /// <summary>
    /// 初始化仓储连接实例
    /// </summary>
    /// <param name="iTenant"></param>
    public static void InitSqlSugarRepository(ITenant iTenant)
    {
        // 主库仓储实例
        var iClientMain = iTenant.GetConnectionScope(SqlSugarConst.MainConfigId);
        iClientAttrDict.TryAdd(typeof(SysTableAttribute), iClientMain);

        // 日志库仓储实例
        var iClientLog = iTenant.IsAnyConnection(SqlSugarConst.LogConfigId)
                ? iTenant.GetConnectionScope(SqlSugarConst.LogConfigId)
                : iTenant.GetConnectionScope(SqlSugarConst.MainConfigId);
        iClientAttrDict.TryAdd(typeof(LogTableAttribute), iClientLog);

        // 其他库仓储实例
        var entityTypes = App.EffectiveTypes.Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass
            && u.IsDefined(typeof(SugarTable), false) && u.GetCustomAttribute<TenantAttribute>() != null).ToList();
        foreach (var entityType in entityTypes)
        {
            MethodInfo genericMethod = typeof(ITenant).GetMethod("GetConnectionScopeWithAttr");
            MethodInfo constructedMethod = genericMethod.MakeGenericMethod(entityType);
            ISqlSugarClient iClientAttr = constructedMethod.Invoke(iTenant, null) as ISqlSugarClient;
            iClientAttrDict.TryAdd(entityType, iClientAttr);
        }
    }

    /// <summary>
    /// 获取指定仓储连接实例
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static ISqlSugarClient GetConnectionScope(Type type)
    {
        iClientAttrDict.TryGetValue(type, out ISqlSugarClient iClient);
        return iClient;
    }
}