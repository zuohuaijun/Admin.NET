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
        dbOptions.ConnectionConfigs.ForEach(config =>
        {
            SetDbConfig(config);
        });

        SqlSugarScope sqlSugar = new(dbOptions.ConnectionConfigs.Adapt<List<ConnectionConfig>>(), db =>
        {
            dbOptions.ConnectionConfigs.ForEach(config =>
            {
                SetDbAop(db.GetConnectionScope(config.ConfigId));
            });
        });

        // 初始化数据库表结构及种子数据
        dbOptions.ConnectionConfigs.ForEach(config =>
        {
            InitDatabase(sqlSugar, config);
        });

        services.AddSingleton<ISqlSugarClient>(sqlSugar); // 单例注册
        services.AddScoped(typeof(SqlSugarRepository<>)); // 仓储注册
        services.AddUnitOfWork<SqlSugarUnitOfWork>(); // 事务与工作单元注册
    }

    /// <summary>
    /// 配置连接属性
    /// </summary>
    /// <param name="config"></param>
    public static void SetDbConfig(DbConnectionConfig config)
    {
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
        config.ConfigureExternalServices = configureExternalServices;
        config.InitKeyType = InitKeyType.Attribute;
        config.IsAutoCloseConnection = true;
        config.MoreSettings = new ConnMoreSettings
        {
            IsAutoRemoveDataCache = true
        };
    }

    /// <summary>
    /// 配置Aop
    /// </summary>
    /// <param name="db"></param>
    public static void SetDbAop(SqlSugarScopeProvider db)
    {
        var config = db.CurrentConnectionConfig;

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
            if (ex.Parametres == null) return;
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

        //// 差异日志
        //db.Aop.OnDiffLogEvent = async u =>
        //{
        //    if (!config.EnableDiffLog) return;

        //    var LogDiff = new SysLogDiff
        //    {
        //        // 操作后记录（字段描述、列名、值、表名、表描述）
        //        AfterData = JsonConvert.SerializeObject(u.AfterData),
        //        // 操作前记录（字段描述、列名、值、表名、表描述）
        //        BeforeData = JsonConvert.SerializeObject(u.BeforeData),
        //        // 传进来的对象
        //        BusinessData = JsonConvert.SerializeObject(u.BusinessData),
        //        // 枚举（insert、update、delete）
        //        DiffType = u.DiffType.ToString(),
        //        Sql = UtilMethods.GetSqlString(config.DbType, u.Sql, u.Parameters),
        //        Parameters = JsonConvert.SerializeObject(u.Parameters),
        //        Duration = u.Time == null ? 0 : (long)u.Time.Value.TotalMilliseconds
        //    };
        //    await db.AsTenant().GetConnectionScope(SqlSugarConst.ConfigId).Insertable(LogDiff).ExecuteCommandAsync();
        //    Console.ForegroundColor = ConsoleColor.Red;
        //    Console.WriteLine(DateTime.Now + $"\r\n**********差异日志开始**********\r\n{Environment.NewLine}{JsonConvert.SerializeObject(LogDiff)}{Environment.NewLine}**********差异日志结束**********\r\n");
        //};

        // 超管时排除各种过滤器
        if (App.User?.FindFirst(ClaimConst.AccountType)?.Value == ((int)AccountTypeEnum.SuperAdmin).ToString())
            return;

        // 配置实体假删除过滤器
        SetDeletedEntityFilter(db);
        // 配置租户过滤器
        SetTenantEntityFilter(db);
        // 配置用户机构范围过滤器
        SetOrgEntityFilter(db);
        // 配置自定义过滤器
        SetCustomEntityFilter(db);
    }

    /// <summary>
    /// 初始化数据库
    /// </summary>
    /// <param name="db"></param>
    /// <param name="config"></param>
    public static void InitDatabase(SqlSugarScope db, DbConnectionConfig config)
    {
        if (!config.EnableInitDb || config.DbType == SqlSugar.DbType.Oracle) return;

        // 创建数据库
        db.DbMaintenance.CreateDatabase();

        // 获取所有实体表-初始化表结构
        var entityTypes = App.EffectiveTypes.Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass
            && u.IsDefined(typeof(SugarTable), false) && !u.IsDefined(typeof(NotTableAttribute), false));
        if (!entityTypes.Any()) return;
        var db2 = db.GetConnectionScope(config.ConfigId);
        foreach (var entityType in entityTypes)
        {
            var tAtt = entityType.GetCustomAttribute<TenantAttribute>();
            if (tAtt != null && tAtt.configId.ToString() != config.ConfigId) continue;
            if (tAtt == null && config.ConfigId != SqlSugarConst.ConfigId) continue;

            var splitTable = entityType.GetCustomAttribute<SplitTableAttribute>();
            if (splitTable == null)
                db2.CodeFirst.InitTables(entityType);
            else
                db2.CodeFirst.SplitTables().InitTables(entityType);
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
            if (tAtt != null && tAtt.configId.ToString() != config.ConfigId) continue;
            if (tAtt == null && config.ConfigId != SqlSugarConst.ConfigId) continue;

            var seedDataTable = seedData.ToList().ToDataTable();
            seedDataTable.TableName = db2.EntityMaintenance.GetEntityInfo(entityType).DbTableName;
            if (seedDataTable.Columns.Contains(SqlSugarConst.PrimaryKey))
            {
                var storage = db2.Storageable(seedDataTable).WhereColumns(SqlSugarConst.PrimaryKey).ToStorage();
                storage.AsInsertable.ExecuteCommand();
                storage.AsUpdateable.ExecuteCommand();
            }
            else // 没有主键或者不是预定义的主键(有重复的可能)
            {
                var storage = db2.Storageable(seedDataTable).ToStorage();
                storage.AsInsertable.ExecuteCommand();
            }
        }
    }

    /// <summary>
    /// 初始化租户库连接
    /// </summary>
    /// <param name="iTenant"></param>
    /// <param name="tenantId"></param>
    public static SqlSugarScopeProvider InitTenantConnection(ITenant iTenant, long tenantId)
    {
        var tenant = App.GetRequiredService<SysCacheService>().Get<List<SysTenant>>(CacheConst.KeyTenant).FirstOrDefault(u => u.Id == tenantId);
        if (!iTenant.IsAnyConnection(tenantId.ToString()))
        {
            iTenant.AddConnection(new ConnectionConfig()
            {
                ConfigId = tenantId.ToString(),
                ConnectionString = tenant.Connection,
                DbType = tenant.DbType,
                IsAutoCloseConnection = true
            });
            SetDbAop(iTenant.GetConnectionScope(tenantId.ToString()));
        }
        return iTenant.GetConnectionScope(tenantId.ToString());
    }

    /// <summary>
    /// 初始化租户业务数据库
    /// </summary>
    /// <param name="itenant"></param>
    /// <param name="config"></param>
    public static void InitTenantDatabase(ITenant itenant, DbConnectionConfig config)
    {
        SetDbConfig(config);

        itenant.AddConnection(config);
        var db = itenant.GetConnectionScope(config.ConfigId);
        db.DbMaintenance.CreateDatabase();

        // 获取所有实体表-初始化租户业务表
        var entityTypes = App.EffectiveTypes.Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass
            && u.IsDefined(typeof(SugarTable), false) && !u.IsDefined(typeof(NotTableAttribute), false)
            && u.IsDefined(typeof(TenantBusinessAttribute), false));
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
    /// 配置实体假删除过滤器
    /// </summary>
    private static void SetDeletedEntityFilter(SqlSugarScopeProvider db)
    {
        // 配置实体假删除缓存
        var cacheKey = $"db:{db.CurrentConnectionConfig.ConfigId}:IsDelete";
        var tableFilterItemList = db.DataCache.Get<List<TableFilterItem<object>>>(cacheKey);
        if (tableFilterItemList == null)
        {
            // 获取基类实体数据表
            var entityTypes = App.EffectiveTypes.Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass
                && (u.BaseType == typeof(EntityBase) || u.BaseType == typeof(EntityTenant) || u.BaseType == typeof(DataEntityBase)));
            if (!entityTypes.Any()) return;

            var tableFilterItems = new List<TableFilterItem<object>>();
            foreach (var entityType in entityTypes)
            {
                // 排除非当前数据库实体
                var tAtt = entityType.GetCustomAttribute<TenantAttribute>();
                if ((tAtt != null && (string)db.CurrentConnectionConfig.ConfigId != tAtt.configId.ToString()) ||
                    (tAtt == null && (string)db.CurrentConnectionConfig.ConfigId != SqlSugarConst.ConfigId))
                    continue;

                Expression<Func<DataEntityBase, bool>> dynamicExpression = u => u.IsDelete == false;
                var tableFilterItem = new TableFilterItem<object>(entityType, dynamicExpression);
                tableFilterItems.Add(tableFilterItem);
                db.QueryFilter.Add(tableFilterItem);
            }
            db.DataCache.Add(cacheKey, tableFilterItems);
        }
        else
        {
            tableFilterItemList.ForEach(u =>
            {
                db.QueryFilter.Add(u);
            });
        }
    }

    /// <summary>
    /// 配置租户过滤器
    /// </summary>
    private static void SetTenantEntityFilter(SqlSugarScopeProvider db)
    {
        var tenantId = App.User?.FindFirst(ClaimConst.TenantId)?.Value;
        if (string.IsNullOrWhiteSpace(tenantId)) return;

        // 配置租户缓存
        var cacheKey = $"db:{db.CurrentConnectionConfig.ConfigId}:TenantId:{tenantId}";
        var tableFilterItemList = db.DataCache.Get<List<TableFilterItem<object>>>(cacheKey);
        if (tableFilterItemList == null)
        {
            // 获取租户实体数据表
            var entityTypes = App.EffectiveTypes.Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass
                && u.BaseType == typeof(EntityTenant));
            if (!entityTypes.Any()) return;

            var tableFilterItems = new List<TableFilterItem<object>>();
            foreach (var entityType in entityTypes)
            {
                // 排除非当前数据库实体
                var tAtt = entityType.GetCustomAttribute<TenantAttribute>();
                if ((tAtt != null && (string)db.CurrentConnectionConfig.ConfigId != tAtt.configId.ToString()) ||
                    (tAtt == null && (string)db.CurrentConnectionConfig.ConfigId != SqlSugarConst.ConfigId))
                    continue;

                Expression<Func<EntityTenant, bool>> dynamicExpression = u => u.TenantId == long.Parse(tenantId);
                var tableFilterItem = new TableFilterItem<object>(entityType, dynamicExpression);
                tableFilterItems.Add(tableFilterItem);
                db.QueryFilter.Add(tableFilterItem);
            }
            db.DataCache.Add(cacheKey, tableFilterItems);
        }
        else
        {
            tableFilterItemList.ForEach(u =>
            {
                db.QueryFilter.Add(u);
            });
        }
    }

    /// <summary>
    /// 配置用户机构范围过滤器
    /// </summary>
    private static void SetOrgEntityFilter(SqlSugarScopeProvider db)
    {
        var userId = App.User?.FindFirst(ClaimConst.UserId)?.Value;
        if (string.IsNullOrWhiteSpace(userId)) return;

        // 配置用户机构范围缓存
        var cacheKey = $"db:{db.CurrentConnectionConfig.ConfigId}:UserId:{userId}";
        var tableFilterItemList = db.DataCache.Get<List<TableFilterItem<object>>>(cacheKey);
        if (tableFilterItemList == null)
        {
            // 获取用户所属机构
            var orgIds = App.GetService<SysCacheService>().Get<List<long>>(CacheConst.KeyOrgIdList + userId);
            if (orgIds == null || orgIds.Count == 0) return;

            // 获取业务实体数据表
            var entityTypes = App.EffectiveTypes.Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass
                && u.BaseType == typeof(DataEntityBase));
            if (!entityTypes.Any()) return;

            var tableFilterItems = new List<TableFilterItem<object>>();
            foreach (var entityType in entityTypes)
            {
                // 排除非当前数据库实体
                var tAtt = entityType.GetCustomAttribute<TenantAttribute>();
                if ((tAtt != null && (string)db.CurrentConnectionConfig.ConfigId != tAtt.configId.ToString()) ||
                    (tAtt == null && (string)db.CurrentConnectionConfig.ConfigId != SqlSugarConst.ConfigId))
                    continue;

                Expression<Func<DataEntityBase, bool>> dynamicExpression = u => orgIds.Contains((long)u.CreateOrgId);
                var tableFilterItem = new TableFilterItem<object>(entityType, dynamicExpression);
                tableFilterItems.Add(tableFilterItem);
                db.QueryFilter.Add(tableFilterItem);
            }
            db.DataCache.Add(cacheKey, tableFilterItems);
        }
        else
        {
            tableFilterItemList.ForEach(u =>
            {
                db.QueryFilter.Add(u);
            });
        }
    }

    /// <summary>
    /// 配置自定义过滤器
    /// </summary>
    private static void SetCustomEntityFilter(SqlSugarScopeProvider db)
    {
        // 配置用户机构范围缓存
        var cacheKey = $"db:{db.CurrentConnectionConfig.ConfigId}:Custom";
        var tableFilterItemList = db.DataCache.Get<List<TableFilterItem<object>>>(cacheKey);
        if (tableFilterItemList == null)
        {
            // 获取自定义实体过滤器
            var entityFilterTypes = App.EffectiveTypes.Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass
                && u.GetInterfaces().Any(i => i.HasImplementedRawGeneric(typeof(IEntityFilter))));
            if (!entityFilterTypes.Any()) return;

            var tableFilterItems = new List<TableFilterItem<object>>();
            foreach (var entityFilter in entityFilterTypes)
            {
                var instance = Activator.CreateInstance(entityFilter);
                var entityFilterMethod = entityFilter.GetMethod("AddEntityFilter");
                var entityFilters = ((IList)entityFilterMethod?.Invoke(instance, null))?.Cast<object>();
                if (entityFilters == null) continue;

                foreach (var u in entityFilters)
                {
                    var tableFilterItem = (TableFilterItem<object>)u;
                    var entityType = tableFilterItem.GetType().GetProperty("type", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(tableFilterItem, null) as Type;
                    // 排除非当前数据库实体
                    var tAtt = entityType.GetCustomAttribute<TenantAttribute>();
                    if ((tAtt != null && (string)db.CurrentConnectionConfig.ConfigId != tAtt.configId.ToString()) ||
                        (tAtt == null && (string)db.CurrentConnectionConfig.ConfigId != SqlSugarConst.ConfigId))
                        return;

                    tableFilterItems.Add(tableFilterItem);
                    db.QueryFilter.Add(tableFilterItem);
                }
            }
            db.DataCache.Add(cacheKey, tableFilterItems);
        }
        else
        {
            tableFilterItemList.ForEach(u =>
            {
                db.QueryFilter.Add(u);
            });
        }
    }
}