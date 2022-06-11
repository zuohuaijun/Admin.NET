namespace Admin.NET.Core.Service;

/// <summary>
/// 系统参数配置服务
/// </summary>
[ApiDescriptionSettings(Name = "系统配置", Order = 193)]
public class SysConfigService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SysConfig> _sysConfigRep;
    private readonly ISysCacheService _sysCacheService;

    public SysConfigService(SqlSugarRepository<SysConfig> sysConfigRep,
        ISysCacheService sysCacheService)
    {
        _sysConfigRep = sysConfigRep;
        _sysCacheService = sysCacheService;
    }

    /// <summary>
    /// 获取参数配置分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet("/sysConfig/pageList")]
    public async Task<SqlSugarPagedList<SysConfig>> GetConfigPageList([FromQuery] PageConfigInput input)
    {
        return await _sysConfigRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Name?.Trim()), u => u.Name.Contains(input.Name))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Code?.Trim()), u => u.Code.Contains(input.Code))
            .OrderBy(u => u.Order).ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 获取参数配置列表
    /// </summary>
    /// <returns></returns>
    [HttpGet("/sysConfig/list")]
    public async Task<List<SysConfig>> GetConfigList()
    {
        return await _sysConfigRep.GetListAsync();
    }

    /// <summary>
    /// 增加参数配置
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/sysConfig/add")]
    public async Task AddConfig(AddConfigInput input)
    {
        var isExist = await _sysConfigRep.IsAnyAsync(u => u.Name == input.Name || u.Code == input.Code);
        if (isExist)
            throw Oops.Oh(ErrorCodeEnum.D9000);

        var config = input.Adapt<SysConfig>();
        await _sysConfigRep.InsertAsync(config);
    }

    /// <summary>
    /// 更新参数配置
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/sysConfig/update")]
    public async Task UpdateConfig(UpdateConfigInput input)
    {
        var isExist = await _sysConfigRep.IsAnyAsync(u => (u.Name == input.Name || u.Code == input.Code) && u.Id != input.Id);
        if (isExist)
            throw Oops.Oh(ErrorCodeEnum.D9000);

        var config = input.Adapt<SysConfig>();
        await _sysConfigRep.AsUpdateable(config).IgnoreColumns(true).ExecuteCommandAsync();

        await _sysCacheService.DelCacheKey(config.Code);
    }

    /// <summary>
    /// 获取参数配置详情
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet("/sysConfig/detail")]
    public async Task<SysConfig> GetConfig([FromQuery] ConfigInput input)
    {
        return await _sysConfigRep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 删除参数配置
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/sysConfig/delete")]
    public async Task DeleteConfig(DeleteConfigInput input)
    {
        var config = await _sysConfigRep.GetFirstAsync(u => u.Id == input.Id);
        if (config.SysFlag == YesNoEnum.Y) // 禁止删除系统参数
            throw Oops.Oh(ErrorCodeEnum.D9001);

        await _sysConfigRep.DeleteAsync(config);

        await _sysCacheService.DelCacheKey(config.Code);
    }

    /// <summary>
    /// 获取参数配置缓存
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    [NonAction]
    public async Task<string> GetConfigCache(string code)
    {
        var value = await _sysCacheService.GetStringAsync(code);
        if (string.IsNullOrEmpty(value))
        {
            var config = await _sysConfigRep.GetFirstAsync(u => u.Code == code);
            value = config != null ? config.Value : "";
            await _sysCacheService.SetStringAsync(code, value);
        }
        return value;
    }
}