// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core.Service;

/// <summary>
/// APIJSON服务
/// </summary>
[ApiDescriptionSettings(Order = 100)]
public class APIJSONService : IDynamicApiController, ITransient
{
    private readonly ISqlSugarClient _db;
    private readonly IdentityService _identityService;
    private readonly TableMapper _tableMapper;
    private readonly SelectTable _selectTable;

    public APIJSONService(ISqlSugarClient db,
        IdentityService identityService,
        TableMapper tableMapper)
    {
        _db = db;
        _tableMapper = tableMapper;
        _identityService = identityService;
        _selectTable = new SelectTable(_identityService, _tableMapper, _db);
    }

    /// <summary>
    /// 统一查询入口
    /// </summary>
    /// <param name="jobject"></param>
    /// <remarks>参数：{"[]":{"SYSLOGOP":{}}}</remarks>
    /// <returns></returns>
    [HttpPost("get")]
    public JObject Query([FromBody] JObject jobject)
    {
        return _selectTable.Query(jobject);
    }

    /// <summary>
    /// 查询表
    /// </summary>
    /// <param name="table"></param>
    /// <param name="jobject"></param>
    /// <returns></returns>
    [HttpPost("get/{table}")]
    public JObject QueryByTable([FromRoute] string table, [FromBody] JObject jobject)
    {
        var ht = new JObject
        {
            { table + "[]", jobject }
        };

        // 自动添加总计数量
        if (jobject["query"] != null && jobject["query"].ToString() != "0" && jobject["total@"] == null)
            ht.Add("total@", "");

        // 每页最大1000条数据
        if (jobject["count"] != null && int.Parse(jobject["count"].ToString()) > 1000)
            throw Oops.Bah("count分页数量最大不能超过1000");

        jobject.Remove("@debug");

        var hasTableKey = false;
        var ignoreConditions = new List<string> { "page", "count", "query" };
        var tableConditions = new JObject(); // 表的其它查询条件，比如过滤、字段等
        foreach (var item in jobject)
        {
            if (item.Key.Equals(table, StringComparison.CurrentCultureIgnoreCase))
            {
                hasTableKey = true;
                break;
            }
            if (!ignoreConditions.Contains(item.Key.ToLower()))
                tableConditions.Add(item.Key, item.Value);
        }

        foreach (var removeKey in tableConditions)
        {
            jobject.Remove(removeKey.Key);
        }

        if (!hasTableKey)
            jobject.Add(table, tableConditions);

        return Query(ht);
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="tables">表对象或数组，若没有传Id则后端生成Id</param>
    /// <returns></returns>
    [HttpPost("add")]
    [UnitOfWork]
    public JObject Add([FromBody] JObject tables)
    {
        var ht = new JObject();
        foreach (var table in tables)
        {
            var talbeName = table.Key.Trim();
            var role = _identityService.GetRole();
            if (!role.Insert.Table.Contains(talbeName, StringComparer.CurrentCultureIgnoreCase))
                throw Oops.Bah($"没权限添加{talbeName}");

            JToken result;
            // 批量插入
            if (table.Value is JArray)
            {
                var ids = new List<object>();
                foreach (var record in table.Value)
                {
                    var cols = record.ToObject<JObject>();
                    var id = _selectTable.InsertSingle(talbeName, cols, role);
                    ids.Add(id);
                }
                result = JToken.FromObject(new { id = ids, count = ids.Count });
            }
            // 单条插入
            else
            {
                var cols = table.Value.ToObject<JObject>();
                var id = _selectTable.InsertSingle(talbeName, cols, role);
                result = JToken.FromObject(new { id });
            }
            ht.Add(talbeName, result);
        }
        return ht;
    }

    /// <summary>
    /// 修改（只支持Id作为条件）
    /// </summary>
    /// <param name="tables">支持多表、多Id批量更新</param>
    /// <returns></returns>
    [HttpPost("update")]
    [UnitOfWork]
    public JObject Edit([FromBody] JObject tables)
    {
        var ht = new JObject();
        foreach (var table in tables)
        {
            var tableName = table.Key.Trim();
            var role = _identityService.GetRole();
            var count = _selectTable.UpdateSingleTable(tableName, table.Value, role);
            ht.Add(tableName, JToken.FromObject(new { count }));
        }
        return ht;
    }

    /// <summary>
    /// 删除（支持非Id条件、支持批量）
    /// </summary>
    /// <param name="tables"></param>
    /// <returns></returns>
    [HttpPost("delete")]
    [UnitOfWork]
    public JObject Delete([FromBody] JObject tables)
    {
        var ht = new JObject();
        var role = _identityService.GetRole();
        foreach (var table in tables)
        {
            var talbeName = table.Key.Trim();
            if (role.Delete == null || role.Delete.Table == null)
                throw Oops.Bah("delete权限未配置");
            if (!role.Delete.Table.Contains(talbeName, StringComparer.CurrentCultureIgnoreCase))
                throw Oops.Bah($"没权限删除{talbeName}");
            //if (!value.ContainsKey("id"))
            //    throw Oops.Bah("未传主键id");

            var value = JObject.Parse(table.Value.ToString());
            var sb = new StringBuilder(100);
            var parameters = new List<SugarParameter>();
            foreach (var f in value)
            {
                if (f.Value is JArray)
                {
                    sb.Append($"{f.Key} in (@{f.Key}) and ");
                    var paraArray = FuncList.TransJArrayToSugarPara(f.Value);
                    parameters.Add(new SugarParameter($"@{f.Key}", paraArray));
                }
                else
                {
                    sb.Append($"{f.Key}=@{f.Key} and ");
                    parameters.Add(new SugarParameter($"@{f.Key}", FuncList.TransJObjectToSugarPara(f.Value)));
                }
            }
            if (!parameters.Any())
                throw Oops.Bah("请输入删除条件");

            var whereSql = sb.ToString().TrimEnd(" and ");
            var count = _db.Deleteable<object>().AS(talbeName).Where(whereSql, parameters).ExecuteCommand(); // 无实体删除
            value.Add("count", count); // 命中数量
            ht.Add(talbeName, value);
        }
        return ht;
    }
}