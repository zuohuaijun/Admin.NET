using APIJSON.NET.Services;
using Newtonsoft.Json.Linq;

namespace Admin.NET.Core.Service.ApiJson;

/// <summary>
/// 系统数据库管理服务
/// </summary>
[ApiDescriptionSettings(Order = 251)]
public class ApiJsonService : IDynamicApiController, ITransient
{
    private SelectTable selectTable;
    private readonly ISqlSugarClient _db;
    private readonly IViewEngine _viewEngine;
    private readonly CodeGenOptions _codeGenOptions;
    //private readonly ApiJsnOptions _roles;
    private readonly IIdentityService _identitySvc;
    private ITableMapper _tableMapper;

    public ApiJsonService(ISqlSugarClient db,
        IViewEngine viewEngine,
        IOptions<CodeGenOptions> codeGenOptions,
        IOptions<ApiJsonOptions> adaOptions,
        IIdentityService identityService,
        ITableMapper tableMapper)
    {
        _db = db;
        _viewEngine = viewEngine;
        //_codeGenOptions = codeGenOptions.Value;
        //_roles = roles.Value;


        _tableMapper = tableMapper;
        _identitySvc = identityService;
        selectTable = new SelectTable(_identitySvc, _tableMapper, _db);
    }

    /// <summary>
    /// ApiJson 统一入口
    /// </summary>
    /// <param name="jobject"></param>
    /// <returns></returns>
    public JObject Post([FromBody] JObject jobject)
    {
        JObject resultJobj = new SelectTable(_identitySvc, _tableMapper, _db).Query(jobject);
        return resultJobj;
    }

}