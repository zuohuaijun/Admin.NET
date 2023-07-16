// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

namespace Admin.NET.Core.Service;

/// <summary>
/// 微信账号服务
/// </summary>
[ApiDescriptionSettings(Order = 220)]
public class SysWechatUserService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SysWechatUser> _sysWechatUserRep;

    public SysWechatUserService(SqlSugarRepository<SysWechatUser> sysWechatUserRep)
    {
        _sysWechatUserRep = sysWechatUserRep;
    }

    /// <summary>
    /// 获取微信用户列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取微信用户列表")]
    public async Task<SqlSugarPagedList<SysWechatUser>> Page(WechatUserInput input)
    {
        return await _sysWechatUserRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.NickName), u => u.NickName.Contains(input.NickName))
            .WhereIF(!string.IsNullOrWhiteSpace(input.PhoneNumber), u => u.Mobile.Contains(input.PhoneNumber))
            .OrderBy(u => u.Id, OrderByType.Desc)
            .ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加微信用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    [DisplayName("增加微信用户")]
    public async Task AddWechatUser(SysWechatUser input)
    {
        await _sysWechatUserRep.InsertAsync(input.Adapt<SysWechatUser>());
    }

    /// <summary>
    /// 更新微信用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    [DisplayName("更新微信用户")]
    public async Task UpdateWechatUser(SysWechatUser input)
    {
        var weChatUser = input.Adapt<SysWechatUser>();
        await _sysWechatUserRep.AsUpdateable(weChatUser).IgnoreColumns(true).ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除微信用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    [DisplayName("删除微信用户")]
    public async Task DeleteWechatUser(DeleteWechatUserInput input)
    {
        await _sysWechatUserRep.DeleteAsync(u => u.Id == input.Id);
    }
}