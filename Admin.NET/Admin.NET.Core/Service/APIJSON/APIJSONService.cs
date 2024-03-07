// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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


    [HttpPost("get/{table}")]
    public async Task<JObject> QueryByTable([FromRoute] string table, [FromBody] JObject jobject)
    {

        JObject ht = new JObject();

        ht.Add(table + "[]", jobject);

        if (jobject["query"] != null && jobject["query"].ToString() != "0" && jobject["total@"] == null)
        {
            //自动添加总计数量
            ht.Add("total@", "");
        }

        //每页最大1000条数据
        if (jobject["count"] != null && int.Parse(jobject["count"].ToString()) > 1000)
        {
            throw Oops.Bah("count分页数量最大不能超过1000");
        }

        bool isDebug = (jobject["@debug"] != null && jobject["@debug"].ToString() != "0");
        jobject.Remove("@debug");

        bool hasTableKey = false;
        List<string> ignoreConditions = new List<string> { "page", "count", "query" };
        JObject tableConditions = new JObject();//表的其它查询条件，比如过滤，字段等
        foreach (var item in jobject)
        {
            if (item.Key.Equals(table, StringComparison.CurrentCultureIgnoreCase))
            {
                hasTableKey = true;
                break;
            }
            if (!ignoreConditions.Contains(item.Key.ToLower()))
            {
                tableConditions.Add(item.Key, item.Value);
            }
        }

        foreach (var removeKey in tableConditions)
        {
            jobject.Remove(removeKey.Key);
        }

        if (!hasTableKey)
        {
            jobject.Add(table, tableConditions);
        }

        return Query(ht);
    }
    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>
    [HttpPost("post")]
    public JObject Add([FromBody] JObject jobject)
    {

        JObject ht = new JObject();

        foreach (var item in jobject)
        {
            string key = item.Key.Trim();
            var role = _identityService.GetRole();
            if (!role.Insert.Table.Contains(key, StringComparer.CurrentCultureIgnoreCase))
            {
                throw Oops.Bah($"没权限添加{key}");
            }
            var dt = new Dictionary<string, object>();
            foreach (var f in JObject.Parse(item.Value.ToString()))
            {
                if (f.Key.ToLower() != "id" && _selectTable.IsCol(key, f.Key) && (role.Insert.Column.Contains("*") || role.Insert.Column.Contains(f.Key, StringComparer.CurrentCultureIgnoreCase)))
                    dt.Add(f.Key, f.Value);
            }
            //todo 自定义id
            int id = _db.Insertable(dt).AS(key).ExecuteReturnIdentity();
            ht.Add(key, JToken.FromObject(new { code = 200, msg = "success", id }));
        }

        return ht;
    }
    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>
    [HttpPost("put")]
    public JObject Edit([FromBody] JObject jobject)
    {
        JObject ht = new JObject();

        foreach (var item in jobject)
        {
            string key = item.Key.Trim();
            var role = _identityService.GetRole();
            if (!role.Update.Table.Contains(key, StringComparer.CurrentCultureIgnoreCase))
            {

                throw Oops.Bah("没权限修改{key}");
            }
            var value = JObject.Parse(item.Value.ToString());
            if (!value.ContainsKey("id"))
            {

                throw Oops.Bah("未传主键id");
            }

            var dt = new Dictionary<string, object>();
            foreach (var f in value)
            {
                if (f.Key.ToLower() != "id" && _selectTable.IsCol(key, f.Key) && (role.Update.Column.Contains("*") || role.Update.Column.Contains(f.Key, StringComparer.CurrentCultureIgnoreCase)))
                {
                    dt.Add(f.Key, f.Value.ToString());
                }
            }
            _db.Updateable(dt).AS(key).Where("id=@id", new { id = value["id"].ToString() }).ExecuteCommand();
            ht.Add(key, JToken.FromObject(new { code = 200, msg = "success", id = value["id"].ToString() }));
        }

        return ht;
    }
    /// <summary>
    /// 删除 支持非id条件,支持批量
    /// </summary>
    /// <param name="jobject"></param>
    /// <returns></returns>
    [HttpPost("delete")]
    public JObject Delete([FromBody] JObject jobject)
    {
        JObject ht = new JObject();
        var role = _identityService.GetRole();
        foreach (var item in jobject)//每个表执行一次
        {
            string talbeName = item.Key.Trim();
            var value = JObject.Parse(item.Value.ToString());//"id":""            
            
            if (role.Delete == null || role.Delete.Table == null)
            {
                throw Oops.Bah("delete权限未配置");
            }
            if (!role.Delete.Table.Contains(talbeName, StringComparer.CurrentCultureIgnoreCase))
            {
                throw Oops.Bah($"没权限删除{talbeName}");
            }
            //if (!value.ContainsKey("id"))
            //{
            //    throw Oops.Bah("未传主键id");
            //}

            var sb = new StringBuilder(100);
            List< SugarParameter > parameters = new List< SugarParameter >();
            foreach (var f in value)//每个条件
            {
                if (f.Value is JArray)//数组
                {
                    sb.Append($"{f.Key} in (@{f.Key}) and ");
                    var paraArray =FuncList.TransJArrayToSugarPara(f.Value);
                    parameters.Add(new SugarParameter($"@{f.Key}", paraArray));
                    //sb.Append($"{f.Key} in ({f.Value.ToString().TrimStart("[").TrimEnd("]").TrimInvisible()})  and");
                }
                else
                {
                    sb.Append($"{f.Key}=@{f.Key} and ");
                    parameters.Add(new SugarParameter($"@{f.Key}", FuncList.TransJObjectToSugarPara(f.Value)));
                }

            }
            string whereSql = sb.ToString().TrimEnd(" and ");
            int count = _db.Deleteable<object>().AS(talbeName).Where(whereSql,parameters).ExecuteCommand();//无实体删除
            value.Add("count", count);//命中数量
            ht.Add(talbeName, value);
            
        }
        return ht;
    }
}