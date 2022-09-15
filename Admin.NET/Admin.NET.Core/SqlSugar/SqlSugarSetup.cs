namespace Admin.NET.Core;

public static class SqlSugarSetup
{
    /// <summary>
    /// Sqlsugar 上下文初始化
    /// </summary>
    /// <param name="services"></param>
    public static void AddSqlSugar(this IServiceCollection services)
    {
        var dbOptions = App.GetOptions<DbConnectionOptions>();
        var configureExternalServices = new ConfigureExternalServices
        {
            EntityService = (type, column) => // 修改列可空-1、带?问号 2、String类型若没有Required
            {
                if ((type.PropertyType.IsGenericType && type.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    || (type.PropertyType == typeof(string) && type.GetCustomAttribute<RequiredAttribute>() == null))
                    column.IsNullable = true;
            },
            DataInfoCacheService = new SqlSugarCache(),
        };
        dbOptions.ConnectionConfigs.ForEach(config =>
        {
            config.ConfigureExternalServices = configureExternalServices;
            config.InitKeyType = InitKeyType.Attribute;
            config.IsAutoCloseConnection = true;
            config.MoreSettings = new ConnMoreSettings
            {
                IsAutoRemoveDataCache = true
            };
        });

        SqlSugarScope sqlSugar = new(dbOptions.ConnectionConfigs.Adapt<List<ConnectionConfig>>(), client =>
        {
            dbOptions.ConnectionConfigs.ForEach(config =>
            {
                var db = client.GetConnectionScope((string)config.ConfigId);

                // 设置超时时间
                db.Ado.CommandTimeOut = 30;

                // 打印SQL语句
                db.Aop.OnLogExecuting = (sql, pars) =>
                {
                    if (sql.StartsWith("SELECT", StringComparison.OrdinalIgnoreCase))
                        Console.ForegroundColor = ConsoleColor.Green;
                    if (sql.StartsWith("UPDATE", StringComparison.OrdinalIgnoreCase) || sql.StartsWith("INSERT", StringComparison.OrdinalIgnoreCase))
                        Console.ForegroundColor = ConsoleColor.White;
                    if (sql.StartsWith("DELETE", StringComparison.OrdinalIgnoreCase))
                        Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("【" + DateTime.Now + "——执行SQL】\r\n" + UtilMethods.GetSqlString(config.DbType, sql, pars) + "\r\n");
                    App.PrintToMiniProfiler("SqlSugar", "Info", sql + "\r\n" + db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                };
                db.Aop.OnError = (ex) =>
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    var pars = db.Utilities.SerializeObject(((SugarParameter[])ex.Parametres).ToDictionary(it => it.ParameterName, it => it.Value));
                    Console.WriteLine("【" + DateTime.Now + "——错误SQL】\r\n" + UtilMethods.GetSqlString(config.DbType, ex.Sql, (SugarParameter[])ex.Parametres) + "\r\n");
                    App.PrintToMiniProfiler("SqlSugar", "Error", $"{ex.Message}{Environment.NewLine}{ex.Sql}{pars}{Environment.NewLine}");
                };

                // 数据审计
                db.Aop.DataExecuting = (oldValue, entityInfo) =>
                {
                    // 新增操作
                    if (entityInfo.OperationType == DataFilterType.InsertByObject)
                    {
                        // 主键(long类型)且没有值的---赋值雪花Id
                        if (entityInfo.EntityColumnInfo.IsPrimarykey && entityInfo.EntityColumnInfo.PropertyInfo.PropertyType == typeof(long))
                        {
                            var id = entityInfo.EntityColumnInfo.PropertyInfo.GetValue(entityInfo.EntityValue);
                            if (id == null || (long)id == 0)
                                entityInfo.SetValue(Yitter.IdGenerator.YitIdHelper.NextId());
                        }
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

                // 差异日志
                db.Aop.OnDiffLogEvent = async u =>
                {
                    if (!config.EnableDiffLog) return;

                    var LogDiff = new SysLogDiff
                    {
                        // 操作后记录（字段描述、列名、值、表名、表描述）
                        AfterData = JsonConvert.SerializeObject(u.AfterData),
                        // 操作前记录（字段描述、列名、值、表名、表描述）
                        BeforeData = JsonConvert.SerializeObject(u.BeforeData),
                        // 传进来的对象
                        BusinessData = JsonConvert.SerializeObject(u.BusinessData),
                        // 枚举（insert、update、delete）
                        DiffType = u.DiffType.ToString(),
                        Sql = UtilMethods.GetSqlString(config.DbType, u.Sql, u.Parameters),
                        Parameters = JsonConvert.SerializeObject(u.Parameters),
                        Duration = u.Time == null ? 0 : (long)u.Time.Value.TotalMilliseconds
                    };
                    await client.GetConnectionScope(SqlSugarConst.ConfigId).Insertable(LogDiff).ExecuteCommandAsync();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(DateTime.Now + $"\r\n**********差异日志开始**********\r\n{Environment.NewLine}{JsonConvert.SerializeObject(LogDiff)}{Environment.NewLine}**********差异日志结束**********\r\n");
                };

                // 缓存处理过滤器
                var queryFilterProvider = db.DataCache.Get<QueryFilterProvider>(config.ConfigId);
                if (queryFilterProvider == null)
                {
                    // 配置实体假删除过滤器
                    SetDeletedEntityFilter(db);
                    // 配置实体机构过滤器
                    SetOrgEntityFilter(db);
                    // 配置自定义实体过滤器
                    SetCustomEntityFilter(db);
                    // 配置租户实体过滤器
                    SetTenantEntityFilter(db);

                    db.DataCache.Add(config.ConfigId, db.QueryFilter);
                }
                else
                {
                    db.QueryFilter = queryFilterProvider;
                }
            });
        });

        // 初始化数据库表结构及种子数据
        InitDataBase(sqlSugar, dbOptions);

        services.AddSingleton<ISqlSugarClient>(sqlSugar); // 单例注册
        services.AddScoped(typeof(SqlSugarRepository<>)); // 注册仓储
        services.AddUnitOfWork<SqlSugarUnitOfWork>(); // 注册事务与工作单元
    }

    /// <summary>
    /// 初始化数据库结构
    /// </summary>
    private static void InitDataBase(SqlSugarScope db, DbConnectionOptions dbOptions)
    {
        // 创建数据库
        dbOptions.ConnectionConfigs.ForEach(config =>
        {
            if (!config.EnableInitDb) return;
            if (config.DbType == SqlSugar.DbType.Oracle) return;
            db.GetConnectionScope(config.ConfigId).DbMaintenance.CreateDatabase();
        });

        // 获取所有实体表-初始化表结构
        var entityTypes = App.EffectiveTypes.Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass
            && u.IsDefined(typeof(SugarTable), false) && !u.IsDefined(typeof(NotTableAttribute), false));
        if (!entityTypes.Any()) return;
        foreach (var entityType in entityTypes)
        {
            var tAtt = entityType.GetCustomAttribute<TenantAttribute>();
            var configId = tAtt == null ? SqlSugarConst.ConfigId : tAtt.configId.ToString();
            if (!dbOptions.ConnectionConfigs.FirstOrDefault(u => u.ConfigId == configId.ToString()).EnableInitDb)
                continue;
            var db2 = db.GetConnectionScope(configId);
            db2.CodeFirst.InitTables(entityType);
        }

        // 获取所有种子配置-初始化数据
        var seedDataTypes = App.EffectiveTypes.Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass
            && u.GetInterfaces().Any(i => i.HasImplementedRawGeneric(typeof(ISqlSugarEntitySeedData<>))));
        if (!seedDataTypes.Any()) return;
        foreach (var seedType in seedDataTypes)
        {
            var instance = Activator.CreateInstance(seedType);

            var hasDataMethod = seedType.GetMethod("HasData");
            var seedData = ((IEnumerable)hasDataMethod?.Invoke(instance, null))?.Cast<object>();
            if (seedData == null) continue;

            var entityType = seedType.GetInterfaces().First().GetGenericArguments().First();
            var tAtt = entityType.GetCustomAttribute<TenantAttribute>();
            var configId = tAtt == null ? SqlSugarConst.ConfigId : tAtt.configId.ToString();
            if (!dbOptions.ConnectionConfigs.FirstOrDefault(u => u.ConfigId == configId).EnableInitDb)
                continue;
            var db2 = db.GetConnectionScope(configId);

            var seedDataTable = seedData.ToList().ToDataTable();
            seedDataTable.TableName = db.EntityMaintenance.GetEntityInfo(entityType).DbTableName;
            if (seedDataTable.Columns.Contains(SqlSugarConst.PrimaryKey))
            {
                var storage = db2.Storageable(seedDataTable).WhereColumns(SqlSugarConst.PrimaryKey).ToStorage();
                storage.AsInsertable.ExecuteCommand();
                storage.AsUpdateable.ExecuteCommand();
            }
            else // 没有主键或者不是预定义的主键(没主键有重复的可能)
            {
                var storage = db2.Storageable(seedDataTable).ToStorage();
                storage.AsInsertable.ExecuteCommand();
            }
        }
    }

    /// <summary>
    /// 配置实体假删除过滤器
    /// </summary>
    private static void SetDeletedEntityFilter(SqlSugarScopeProvider db)
    {
        // 获取所有继承基类数据表集合
        var entityTypes = App.EffectiveTypes.Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass
            && (u.BaseType == typeof(EntityBase) || u.BaseType == typeof(EntityTenant) || u.BaseType == typeof(DataEntityBase)));
        if (!entityTypes.Any()) return;

        foreach (var entityType in entityTypes)
        {
            // 排除非当前数据库实体
            var tAtt = entityType.GetCustomAttribute<TenantAttribute>();
            if ((tAtt != null && (string)db.CurrentConnectionConfig.ConfigId != tAtt.configId.ToString()) ||
                (tAtt == null && (string)db.CurrentConnectionConfig.ConfigId != SqlSugarConst.ConfigId))
                continue;

            Expression<Func<DataEntityBase, bool>> dynamicExpression = u => u.IsDelete == false;
            db.QueryFilter.Add(new TableFilterItem<object>(entityType, dynamicExpression));
        }
    }

    /// <summary>
    /// 配置实体机构过滤器
    /// </summary>
    private static async void SetOrgEntityFilter(SqlSugarScopeProvider db)
    {
        // 获取业务数据表集合
        var dataEntityTypes = App.EffectiveTypes.Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass
            && u.BaseType == typeof(DataEntityBase));
        if (!dataEntityTypes.Any()) return;

        var userId = App.User?.FindFirst(ClaimConst.UserId)?.Value;
        if (string.IsNullOrWhiteSpace(userId)) return;

        // 获取用户机构Id集合
        var orgIds = await App.GetService<SysCacheService>().GetOrgIdList(long.Parse(userId));
        if (orgIds == null || orgIds.Count == 0) return;

        foreach (var dataEntityType in dataEntityTypes)
        {
            Expression<Func<DataEntityBase, bool>> dynamicExpression = u => orgIds.Contains((long)u.CreateOrgId);
            db.QueryFilter.Add(new TableFilterItem<object>(dataEntityType, dynamicExpression));
        }
    }

    /// <summary>
    /// 配置自定义实体过滤器
    /// </summary>
    private static void SetCustomEntityFilter(SqlSugarScopeProvider db)
    {
        // 排除超管过滤
        if (App.User?.FindFirst(ClaimConst.SuperAdmin)?.Value == ((int)UserTypeEnum.SuperAdmin).ToString())
            return;

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
    private static void SetTenantEntityFilter(SqlSugarScopeProvider db)
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
}