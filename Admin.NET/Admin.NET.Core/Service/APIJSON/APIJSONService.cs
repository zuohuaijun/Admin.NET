// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

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

    public APIJSONService(ISqlSugarClient db,
        IdentityService identityService,
        TableMapper tableMapper)
    {
        _db = db;
        _tableMapper = tableMapper;
        _identityService = identityService;
    }

    /// <summary>
    /// 统一入口
    /// </summary>
    /// <param name="jobject"></param>
    /// <remarks>参数：{"[]":{"SYS_LOG_OP":{}}}</remarks>
    /// <returns></returns>
    public JObject Post([FromBody] JObject jobject)
    {
        return new SelectTable(_identityService, _tableMapper, _db).Query(jobject);
    }
}