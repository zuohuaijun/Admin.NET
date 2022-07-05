namespace Admin.NET.Core.Service;

/// <summary>
/// 系统数据资源服务
/// </summary>
[ApiDescriptionSettings(Name = "系统数据资源", Order = 189)]
public class SysDataResourceService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SysDataResource> _sysDataResourceRep;

    public SysDataResourceService(SqlSugarRepository<SysDataResource> sysDataResourceRep)
    {
        _sysDataResourceRep = sysDataResourceRep;
    }

    /// <summary>
    /// 获取数据资源列表
    /// </summary>
    /// <returns></returns>
    [HttpGet("/sysDataResource/list")]
    public async Task<List<SysDataResource>> GetDataResourceList([FromQuery] DataResourceInput input)
    {
        var idList = input.Id > 0 ? await GetChildIdListWithSelfById(input.Id) : new List<long>();

        var iSugarQueryable = _sysDataResourceRep.AsQueryable().OrderBy(u => u.Order)
            .WhereIF(idList.Count > 0, u => idList.Contains(u.Id)); // 非超级管理员限制

        if (!string.IsNullOrWhiteSpace(input.Name) || !string.IsNullOrWhiteSpace(input.Code) || !string.IsNullOrWhiteSpace(input.Value) || input.Id > 0)
        {
            return await iSugarQueryable
                .WhereIF(!string.IsNullOrWhiteSpace(input.Name), u => u.Name.Contains(input.Name))
                .WhereIF(!string.IsNullOrWhiteSpace(input.Value), u => u.Value.Contains(input.Value))
                .WhereIF(!string.IsNullOrWhiteSpace(input.Code), u => u.Code.Contains(input.Code))
                .ToListAsync();
        }
        return await iSugarQueryable.ToTreeAsync(u => u.Children, u => u.Pid, input.Id > 0 ? input.Id : 0);
    }

    /// <summary>
    /// 增加数据资源
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/sysDataResource/add")]
    public async Task<long> AddDataResource(AddDataResourceInput input)
    {
        var isExist = await _sysDataResourceRep.IsAnyAsync(u => u.Code == input.Code && u.Name == input.Name);
        if (isExist)
            throw Oops.Oh(ErrorCodeEnum.D1602);

        var newCode = "";
        // 生成编码Code和排序(每级2位编码)
        var sysDataResource = await _sysDataResourceRep.AsQueryable().OrderByDescending(o => o.Code)
            .FirstAsync(u => u.Pid == input.Pid);

        if (sysDataResource != null)
        {
            newCode = sysDataResource.Code[0..^3] + string.Format("{0:d3}", int.Parse(sysDataResource.Code[^3..]) + 1);
        }
        else
        {
            //如果没有根节点，默认为1001
            if (input.Pid == 0)
            {
                newCode = "1001";
            }
            else
            {
                sysDataResource = await _sysDataResourceRep.GetFirstAsync(u => u.Id == input.Pid);
                newCode = sysDataResource.Code + "01";
            }
        }
        var newDataResource = input.Adapt<SysDataResource>();
        newDataResource.Code = newCode;
        newDataResource.Order = int.Parse(newCode[^3..]);
        newDataResource = await _sysDataResourceRep.AsInsertable(newDataResource).ExecuteReturnEntityAsync();

        return newDataResource.Id;
    }

    /// <summary>
    /// 更新数据资源
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/sysDataResource/update")]
    [UnitOfWork]
    public async Task UpdateDataResource(UpdateDataResourceInput input)
    {
        if (input.Pid != 0)
        {
            var pDataResource = await _sysDataResourceRep.GetFirstAsync(u => u.Id == input.Pid);
            _ = pDataResource ?? throw Oops.Oh(ErrorCodeEnum.D1600);
        }
        if (input.Id == input.Pid)
            throw Oops.Oh(ErrorCodeEnum.D1601);

        var sysDataResource = await _sysDataResourceRep.GetFirstAsync(u => u.Id == input.Id);
        var isExist = await _sysDataResourceRep.IsAnyAsync(u => (u.Name == input.Name && u.Code == input.Code) && u.Id != sysDataResource.Id);
        if (isExist)
            throw Oops.Oh(ErrorCodeEnum.D1602);

        // 父Id不能为自己的子节点
        var childIdList = await GetChildIdListWithSelfById(input.Id);
        if (childIdList.Contains(input.Pid))
            throw Oops.Oh(ErrorCodeEnum.D1601);

        var dataResource = input.Adapt<SysDataResource>();
        await _sysDataResourceRep.AsUpdateable(dataResource).IgnoreColumns(true).ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除数据资源
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/sysDataResource/delete")]
    public async Task DeleteDataResource(DeleteDataResourceInput input)
    {
        var sysDataResource = await _sysDataResourceRep.GetFirstAsync(u => u.Id == input.Id);

        // 获取本节点对应所有子节点id列表
        var treeList = await _sysDataResourceRep.AsQueryable().ToChildListAsync(u => u.Pid, input.Id);
        var idList = treeList.Select(u => u.Id).ToList();

        // 级联删除数据资源子节点
        await _sysDataResourceRep.DeleteAsync(u => idList.Contains(u.Id));
    }

    /// <summary>
    /// 根据节点Id获取子节点Id集合(包含自己)
    /// </summary>
    /// <param name="pid"></param>
    /// <returns></returns>
    private async Task<List<long>> GetChildIdListWithSelfById(long pid)
    {
        var treeList = await _sysDataResourceRep.AsQueryable().ToChildListAsync(u => u.Pid, pid);
        return treeList.Select(u => u.Id).ToList();
    }
}