using DbType = SqlSugar.DbType;

namespace Admin.NET.Core;

public static class SqlSugarSetup
{
    /// <summary>
    /// Sqlsugar上下文初始化
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void AddSqlSugarSetup(this IServiceCollection services, IConfiguration configuration)
    {
        // SqlSugarScope用AddSingleton单例
        services.AddSingleton<ISqlSugarClient>(provider =>
        {
            var dbOptions = App.GetOptions<ConnectionStringsOptions>();
            DealConnectionStr(ref dbOptions); // 处理本地库根目录路径

            var connectionConfigs = SqlSugarConst.ConnectionConfigs; // 方便多库生成
            var configureExternalServices = new ConfigureExternalServices
            {
                EntityService = (type, column) => // 修改列可空
                {
                    // 1、带?问号 2、String类型若没有Required
                    if ((type.PropertyType.IsGenericType && type.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        || (type.PropertyType == typeof(string) && type.GetCustomAttribute<RequiredAttribute>() == null))
                        column.IsNullable = true;
                },
            };
            var defaultConnection = new ConnectionConfig()
            {
                DbType = (DbType)Convert.ToInt32(Enum.Parse(typeof(DbType), dbOptions.DefaultDbType)),
                ConnectionString = dbOptions.DefaultConnection,
                IsAutoCloseConnection = true,
                ConfigId = dbOptions.DefaultConfigId,
                ConfigureExternalServices = configureExternalServices
            };
            connectionConfigs.Add(defaultConnection);

            dbOptions.DbConfigs.ForEach(config =>
            {
                var connection = new ConnectionConfig()
                {
                    DbType = (DbType)Convert.ToInt32(Enum.Parse(typeof(DbType), config.DbType)),
                    ConnectionString = config.DbConnection,
                    IsAutoCloseConnection = true,
                    ConfigId = config.DbConfigId,
                    ConfigureExternalServices = configureExternalServices
                };
                connectionConfigs.Add(connection);
            });

            SqlSugarScope sqlSugar = new(connectionConfigs, db =>
            {
                connectionConfigs.ForEach(config =>
                {
                    var dbProvider = db.GetConnection((string)config.ConfigId);

                    // 设置超时时间
                    dbProvider.Ado.CommandTimeOut = 30;

                    // 打印SQL语句
                    dbProvider.Aop.OnLogExecuting = (sql, pars) =>
                    {
                        if (sql.StartsWith("SELECT"))
                            Console.ForegroundColor = ConsoleColor.Green;
                        if (sql.StartsWith("UPDATE") || sql.StartsWith("INSERT"))
                            Console.ForegroundColor = ConsoleColor.White;
                        if (sql.StartsWith("DELETE"))
                            Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine(sql + "\r\n" + db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                        App.PrintToMiniProfiler("SqlSugar", "Info", sql + "\r\n" + db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                    };
                    dbProvider.Aop.OnError = (ex) =>
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        var pars = db.Utilities.SerializeObject(((SugarParameter[])ex.Parametres).ToDictionary(it => it.ParameterName, it => it.Value));
                        Console.WriteLine($"{ex.Message}{Environment.NewLine}{ex.Sql}{Environment.NewLine}{pars}{Environment.NewLine}");
                        App.PrintToMiniProfiler("SqlSugar", "Error", $"{ex.Message}{Environment.NewLine}{ex.Sql}{pars}{Environment.NewLine}");
                    };

                    // 数据审计
                    dbProvider.Aop.DataExecuting = (oldValue, entityInfo) =>
                    {
                        // 新增操作
                        if (entityInfo.OperationType == DataFilterType.InsertByObject)
                        {
                            // 主键(long)-赋值雪花Id
                            if (entityInfo.EntityColumnInfo.IsPrimarykey && entityInfo.EntityColumnInfo.PropertyInfo.PropertyType == typeof(long))
                                entityInfo.SetValue(Yitter.IdGenerator.YitIdHelper.NextId());
                            if (entityInfo.PropertyName == "CreateTime")
                                entityInfo.SetValue(DateTime.Now);
                            if (App.User != null)
                            {
                                if (entityInfo.PropertyName == "TenantId")
                                {
                                    var tenantId = ((dynamic)entityInfo.EntityValue).TenantId;
                                    if (tenantId == null || tenantId == 0)
                                        entityInfo.SetValue(App.User.FindFirst(ClaimConst.TenantId)?.Value);
                                }
                                if (entityInfo.PropertyName == "CreateUserId")
                                    entityInfo.SetValue(App.User.FindFirst(ClaimConst.UserId)?.Value);
                                if (entityInfo.PropertyName == "CreateOrgId")
                                    entityInfo.SetValue(App.User.FindFirst(ClaimConst.OrgId)?.Value);
                            }
                        }
                        // 更新操作
                        if (entityInfo.OperationType == DataFilterType.UpdateByObject)
                        {
                            if (entityInfo.PropertyName == "UpdateTime")
                                entityInfo.SetValue(DateTime.Now);
                            if (entityInfo.PropertyName == "UpdateUserId")
                                entityInfo.SetValue(App.User?.FindFirst(ClaimConst.UserId)?.Value);
                        }
                    };

                    // 配置实体假删除过滤器
                    SetDeletedEntityFilter(dbProvider);
                    // 配置实体机构过滤器
                    SetOrgEntityFilter(dbProvider);
                    // 配置自定义实体过滤器
                    SetCustomEntityFilter(dbProvider);
                    // 配置租户实体过滤器
                    SetTenantEntityFilter(dbProvider);
                });
            });

            // 初始化数据库结构及种子数据
            if (dbOptions.InitTable)
                InitDataBase(sqlSugar, dbOptions);
            return sqlSugar;
        });
        services.AddScoped(typeof(SqlSugarRepository<>)); // 注册仓储
    }

    /// <summary>
    /// 初始化数据库结构
    /// </summary>
    public static void InitDataBase(SqlSugarScope db, ConnectionStringsOptions dbOptions)
    {
        // 创建系统默认数据库
        db.DbMaintenance.CreateDatabase();
        // 创建其他业务数据库
        dbOptions.DbConfigs.ForEach(config =>
        {
            db.GetConnection(config.DbConfigId).DbMaintenance.CreateDatabase();
        });

        // 获取所有实体表
        var entityTypes = App.EffectiveTypes.Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass
        && u.IsDefined(typeof(SqlSugarEntityAttribute), false))
        .OrderByDescending(u => u.GetSqlSugarEntityOrder());
        if (!entityTypes.Any()) return;
        // 初始化库表结构
        foreach (var entityType in entityTypes)
        {
            var dbConfigId = entityType.GetCustomAttribute<SqlSugarEntityAttribute>(true).DbConfigId;
            db.ChangeDatabase(dbConfigId);
            db.CodeFirst.InitTables(entityType);
        }

        // 获取所有实体种子数据
        var seedDataTypes = App.EffectiveTypes.Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass
            && u.GetInterfaces().Any(i => i.HasImplementedRawGeneric(typeof(ISqlSugarEntitySeedData<>))));
        if (!seedDataTypes.Any()) return;

        foreach (var seedType in seedDataTypes)
        {
            var instance = Activator.CreateInstance(seedType);

            var hasDataMethod = seedType.GetMethod("HasData");
            var seedData = ((IList)hasDataMethod?.Invoke(instance, null))?.Cast<object>();
            if (seedData == null) continue;

            var entityType = seedType.GetInterfaces().First().GetGenericArguments().First();
            var dbConfigId = entityType.GetCustomAttribute<SqlSugarEntityAttribute>(true).DbConfigId;
            db.ChangeDatabase(dbConfigId);

            var seedDataTable = seedData.ToList().ToDataTable();
            if (seedDataTable.Columns.Contains(SqlSugarConst.PrimaryKey))
            {
                var storage = db.Storageable(seedDataTable).WhereColumns(SqlSugarConst.PrimaryKey).ToStorage();
                storage.AsInsertable.ExecuteCommand();
                storage.AsUpdateable.ExecuteCommand();
            }
            else //没有主键或者不是预定义的主键(没主键有重复的可能)
            {
                var storage = db.Storageable(seedDataTable).ToStorage();
                storage.AsInsertable.ExecuteCommand();
            }
        }
    }

    /// <summary>
    /// 配置实体假删除过滤器
    /// </summary>
    public static void SetDeletedEntityFilter(SqlSugarProvider db)
    {
        // 获取所有继承基类数据表集合
        var entityTypes = App.EffectiveTypes.Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass
            && u.BaseType == typeof(EntityBase));
        if (!entityTypes.Any()) return;

        foreach (var entityType in entityTypes)
        {
            Expression<Func<DataEntityBase, bool>> dynamicExpression = u => u.IsDelete == false;
            db.QueryFilter.Add(new TableFilterItem<object>(entityType, dynamicExpression));
        }
    }

    /// <summary>
    /// 配置实体机构过滤器
    /// </summary>
    public static async void SetOrgEntityFilter(SqlSugarProvider db)
    {
        // 获取业务数据表集合
        var dataEntityTypes = App.EffectiveTypes.Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass
            && u.BaseType == typeof(DataEntityBase));
        if (!dataEntityTypes.Any()) return;

        var userId = App.User?.FindFirst(ClaimConst.UserId)?.Value;
        if (string.IsNullOrWhiteSpace(userId)) return;

        // 获取用户机构Id集合
        var orgIds = await App.GetService<SysCacheService>().GetOrgIdList(long.Parse(userId));
        if (orgIds == null) return;

        foreach (var dataEntityType in dataEntityTypes)
        {
            Expression<Func<DataEntityBase, bool>> dynamicExpression = u => orgIds.Contains((long)u.CreateOrgId);
            db.QueryFilter.Add(new TableFilterItem<object>(dataEntityType, dynamicExpression));
        }
    }

    /// <summary>
    /// 配置自定义实体过滤器
    /// </summary>
    public static void SetCustomEntityFilter(SqlSugarProvider db)
    {
        // 获取继承自定义实体过滤器接口的类集合
        var entityFilterTypes = App.EffectiveTypes.Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass
            && u.GetInterfaces().Any(i => i.HasImplementedRawGeneric(typeof(IEntityFilter))));
        if (!entityFilterTypes.Any()) return;

        foreach (var entityFilter in entityFilterTypes)
        {
            var instance = Activator.CreateInstance(entityFilter);
            var entityFilterMethod = entityFilter.GetMethod("AddEntityFilter");
            var entityFilters = ((IList)entityFilterMethod?.Invoke(instance, null))?.Cast<object>();
            if (entityFilters == null) continue;
            foreach (TableFilterItem<object> filter in entityFilters)
                db.QueryFilter.Add(filter);
        }
    }

    /// <summary>
    /// 配置租户实体过滤器
    /// </summary>
    public static void SetTenantEntityFilter(SqlSugarProvider db)
    {
        // 获取租户实体数据表集合
        var dataEntityTypes = App.EffectiveTypes.Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass
            && u.BaseType == typeof(EntityTenant));
        if (!dataEntityTypes.Any()) return;

        var tenantId = App.User?.FindFirst(ClaimConst.TenantId)?.Value;
        if (string.IsNullOrWhiteSpace(tenantId)) return;

        foreach (var dataEntityType in dataEntityTypes)
        {
            Expression<Func<EntityTenant, bool>> dynamicExpression = u => u.TenantId == long.Parse(tenantId);
            db.QueryFilter.Add(new TableFilterItem<object>(dataEntityType, dynamicExpression));
        }
    }

    /// <summary>
    /// 处理本地库根目录路径
    /// </summary>
    /// <param name="dbOptions"></param>
    private static void DealConnectionStr(ref ConnectionStringsOptions dbOptions)
    {
        if (dbOptions.DefaultDbType.Trim().ToLower() == "sqlite" && dbOptions.DefaultConnection.Contains("./"))
        {
            dbOptions.DefaultConnection = UpdateDbPath(dbOptions.DefaultConnection);
        }
        dbOptions.DbConfigs.ForEach(cofing =>
        {
            if (cofing.DbType.Trim().ToLower() == "sqlite" && cofing.DbConnection.Contains("./"))
                cofing.DbConnection = UpdateDbPath(cofing.DbConnection);
        });
    }

    private static string UpdateDbPath(string dbConnection)
    {
        var file = Path.GetFileName(dbConnection.Replace("DataSource=", ""));
        return $"DataSource={Environment.CurrentDirectory.Replace(@"\bin\Debug", "")}/{file}";
    }
}