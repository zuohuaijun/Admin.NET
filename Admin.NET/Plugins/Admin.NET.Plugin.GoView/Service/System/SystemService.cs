using LoginInput = Admin.NET.Plugin.GoView.Service.Dto.LoginInput;
using LoginOutput = Admin.NET.Plugin.GoView.Service.Dto.LoginOutput;

namespace Admin.NET.Plugin.GoView.Service;

/// <summary>
/// 系统登录服务
/// </summary>
[UnifyProvider("GoView")]
[ApiDescriptionSettings(GoViewConst.GroupName, Order = 500)]
public class SystemService : IDynamicApiController
{
    private readonly SysAuthService _sysAuthService;
    private readonly SqlSugarRepository<SysUser> _userRep;

    public SystemService(SysAuthService sysAuthService, SqlSugarRepository<SysUser> userRep)
    {
        _sysAuthService = sysAuthService;
        _userRep = userRep;
    }

    /// <summary>
    /// GoView 登录
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [DisplayName("GoView 登录")]
    public async Task<LoginOutput> Login(LoginInput input)
    {
        var loginResult = await _sysAuthService.Login(new Core.Service.LoginInput()
        {
            Account = input.Username,
            Password = input.Password,
        });

        var user = await _userRep.AsQueryable().Includes(t => t.SysOrg).Filter(null, true).FirstAsync(u => u.Account.Equals(input.Username));

        return new LoginOutput()
        {
            UserInfo = new LoginUserInfo
            {
                Id = user.Id + "",
                Username = user.Account,
                Nickname = user.NickName,
            },
            Token = new LoginToken
            {
                TokenValue = $"Bearer {loginResult.AccessToken}",
            }
        };
    }

    /// <summary>
    /// GoView 退出
    /// </summary>
    [HttpGet]
    [DisplayName("GoView 退出")]
    public void Logout()
    {
        _sysAuthService.Logout();
    }

    /// <summary>
    /// 获取 OSS 上传接口
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [ApiDescriptionSettings(Name = "GetOssInfo")]
    [DisplayName("获取 OSS 上传接口")]
    public Task<OssUrlOutput> GetOssInfo()
    {
        return Task.FromResult(new OssUrlOutput { BucketUrl = "" });
    }
}