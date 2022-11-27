namespace Admin.NET.Core.Service;

/// <summary>
/// 微信账号服务
/// </summary>
[ApiDescriptionSettings(Order = 100)]
public class WeChatUserService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<WeChatUser> _weChatUserRep;

    public WeChatUserService(SqlSugarRepository<WeChatUser> weChatUserRep)
    {
        _weChatUserRep = weChatUserRep;
    }

    /// <summary>
    /// 获取微信用户列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet("/weChatUser/page")]
    public async Task<SqlSugarPagedList<WeChatUser>> GetWeChatUserPage([FromQuery] WeChatUserInput input)
    {
        return await _weChatUserRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.NickName), u => u.NickName.Contains(input.NickName))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Mobile), u => u.Mobile.Contains(input.Mobile))
            .OrderBy(u => u.Id, OrderByType.Desc)
            .ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加微信用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/weChatUser/add")]
    public async Task AddWeChatUser(WeChatUser input)
    {
        await _weChatUserRep.InsertAsync(input.Adapt<WeChatUser>());
    }

    /// <summary>
    /// 更新微信用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/weChatUser/update")]
    public async Task UpdateWeChatUser(WeChatUser input)
    {
        var weChatUser = input.Adapt<WeChatUser>();
        await _weChatUserRep.AsUpdateable(weChatUser).IgnoreColumns(true).ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除微信用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("/weChatUser/delete")]
    public async Task DeleteWeChatUser(DeleteWeChatUserInput input)
    {
        await _weChatUserRep.DeleteAsync(u => u.Id == input.Id);
    }
}