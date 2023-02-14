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
    /// <returns></returns>
    public JObject Post([FromBody] JObject jobject)
    {
        return new SelectTable(_identityService, _tableMapper, _db).Query(jobject);
    }
}