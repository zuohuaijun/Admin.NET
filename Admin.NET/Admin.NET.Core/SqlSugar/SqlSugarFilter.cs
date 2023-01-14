namespace Admin.NET.Core;

public static class SqlSugarFilter
{
    /// <summary>
    /// 缓存全局查询过滤器（内存缓存）
    /// </summary>
    private static readonly ICache _cache = Cache.Default;

    /// <summary>
    /// 配置实体假删除过滤器
    /// </summary>
    public static void SetDeletedEntityFilter(SqlSugarScopeProvider db)
    {
        var cacheKey = $"db:{db.CurrentConnectionConfig.ConfigId}:IsDelete";
        var deletedFilter = _cache.Get<ConcurrentDictionary<Type, LambdaExpression>>(cacheKey);
        if (deletedFilter == null)
        {
            // 获取基类实体数据表
            var entityTypes = App.EffectiveTypes.Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass
                && (u.BaseType == typeof(EntityBase) || u.BaseType == typeof(EntityTenant) || u.BaseType == typeof(EntityBaseData)));
            if (!entityTypes.Any()) return;

            deletedFilter = new ConcurrentDictionary<Type, LambdaExpression>();
            foreach (var entityType in entityTypes)
            {
                // 排除非当前数据库实体
                var tAtt = entityType.GetCustomAttribute<TenantAttribute>();
                if ((tAtt != null && db.CurrentConnectionConfig.ConfigId.ToString() != tAtt.configId.ToString()) ||
                    (tAtt == null && db.CurrentConnectionConfig.ConfigId.ToString() != SqlSugarConst.ConfigId))
                    continue;

                var lambda = DynamicExpressionParser.ParseLambda(new[] {
                    Expression.Parameter(entityType, "u") }, typeof(bool), $"{nameof(EntityBase.IsDelete)} == @0", false);
                db.QueryFilter.AddTableFilter(entityType, lambda);
                deletedFilter.TryAdd(entityType, lambda);
            }
            _cache.Add(cacheKey, deletedFilter);
        }
        else
        {
            foreach (var filter in deletedFilter)
                db.QueryFilter.AddTableFilter(filter.Key, filter.Value);
        }
    }

    /// <summary>
    /// 配置租户过滤器
    /// </summary>
    public static void SetTenantEntityFilter(SqlSugarScopeProvider db)
    {
        var tenantId = App.User?.FindFirst(ClaimConst.TenantId)?.Value;
        if (string.IsNullOrWhiteSpace(tenantId)) return;

        // 配置租户缓存
        var cacheKey = $"db:{db.CurrentConnectionConfig.ConfigId}:TenantId:{tenantId}";
        var tenantFilter = _cache.Get<ConcurrentDictionary<Type, LambdaExpression>>(cacheKey);
        if (tenantFilter == null)
        {
            // 获取租户实体数据表
            var entityTypes = App.EffectiveTypes.Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass
                && (u.BaseType == typeof(EntityTenant) || u.BaseType == typeof(EntityTenantId)));
            if (!entityTypes.Any()) return;

            tenantFilter = new ConcurrentDictionary<Type, LambdaExpression>();
            foreach (var entityType in entityTypes)
            {
                // 获取库隔离租户业务实体
                var systemTableAtt = entityType.GetCustomAttribute<SystemTableAttribute>();
                if (systemTableAtt != null)
                {
                    // 排除非当前数据库实体
                    var tenantAtt = entityType.GetCustomAttribute<TenantAttribute>();
                    if ((tenantAtt != null && db.CurrentConnectionConfig.ConfigId.ToString() != tenantAtt.configId.ToString()) ||
                        (tenantAtt == null && db.CurrentConnectionConfig.ConfigId.ToString() != SqlSugarConst.ConfigId))
                        continue;
                }

                var lambda = DynamicExpressionParser.ParseLambda(new[] {
                    Expression.Parameter(entityType, "u") }, typeof(bool), $"{nameof(EntityTenant.TenantId)} == @0", long.Parse(tenantId));
                db.QueryFilter.AddTableFilter(entityType, lambda);
                tenantFilter.TryAdd(entityType, lambda);
            }
            _cache.Add(cacheKey, tenantFilter);
        }
        else
        {
            foreach (var filter in tenantFilter)
                db.QueryFilter.AddTableFilter(filter.Key, filter.Value);
        }
    }

    /// <summary>
    /// 配置用户机构范围过滤器
    /// </summary>
    public static void SetOrgEntityFilter(SqlSugarScopeProvider db)
    {
        var userId = App.User?.FindFirst(ClaimConst.UserId)?.Value;
        if (string.IsNullOrWhiteSpace(userId)) return;

        // 配置用户机构范围缓存
        var cacheKey = $"db:{db.CurrentConnectionConfig.ConfigId}:UserId:{userId}";
        var orgFilter = _cache.Get<ConcurrentDictionary<Type, LambdaExpression>>(cacheKey);
        if (orgFilter == null)
        {
            // 获取用户所属机构
            var orgIds = App.GetService<SysCacheService>().Get<List<long>>(CacheConst.KeyOrgIdList + userId);
            if (orgIds == null || orgIds.Count == 0) return;

            // 获取业务实体数据表
            var entityTypes = App.EffectiveTypes.Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass
                && u.BaseType == typeof(EntityBaseData));
            if (!entityTypes.Any()) return;

            orgFilter = new ConcurrentDictionary<Type, LambdaExpression>();
            foreach (var entityType in entityTypes)
            {
                // 排除非当前数据库实体
                var tAtt = entityType.GetCustomAttribute<TenantAttribute>();
                if ((tAtt != null && db.CurrentConnectionConfig.ConfigId.ToString() != tAtt.configId.ToString()) ||
                    (tAtt == null && db.CurrentConnectionConfig.ConfigId.ToString() != SqlSugarConst.ConfigId))
                    continue;

                var lambda = DynamicExpressionParser.ParseLambda(new[] {
                    Expression.Parameter(entityType, "u") }, typeof(bool), $"@0.Contains(u.{nameof(EntityBaseData.CreateOrgId)}??{default(long)})", orgIds);
                db.QueryFilter.AddTableFilter(entityType, lambda);
                orgFilter.TryAdd(entityType, lambda);
            }
            _cache.Add(cacheKey, orgFilter);
        }
        else
        {
            foreach (var filter in orgFilter)
                db.QueryFilter.AddTableFilter(filter.Key, filter.Value);
        }
    }

    /// <summary>
    /// 配置自定义过滤器
    /// </summary>
    public static void SetCustomEntityFilter(SqlSugarScopeProvider db)
    {
        // 配置用户机构范围缓存
        var cacheKey = $"db:{db.CurrentConnectionConfig.ConfigId}:Custom";
        var tableFilterItemList = _cache.Get<List<TableFilterItem<object>>>(cacheKey);
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
                    if ((tAtt != null && db.CurrentConnectionConfig.ConfigId.ToString() != tAtt.configId.ToString()) ||
                        (tAtt == null && db.CurrentConnectionConfig.ConfigId.ToString() != SqlSugarConst.ConfigId))
                        return;

                    tableFilterItems.Add(tableFilterItem);
                    db.QueryFilter.Add(tableFilterItem);
                }
            }
            _cache.Add(cacheKey, tableFilterItems);
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