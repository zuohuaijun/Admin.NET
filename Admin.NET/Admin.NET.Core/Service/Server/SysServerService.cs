namespace Admin.NET.Core.Service;

/// <summary>
/// 系统服务器监控服务
/// </summary>
[ApiDescriptionSettings(Order = 185)]
public class SysServerService : IDynamicApiController, ITransient
{
    public SysServerService()
    {
    }

    /// <summary>
    /// 服务器基本配置
    /// </summary>
    /// <returns></returns>
    [HttpGet("/server/base")]
    public async Task<dynamic> GetServerBaseInfo()
    {
        return await Task.FromResult(ServerUtil.GetServerBaseInfo());
    }

    /// <summary>
    /// 服务器使用资源
    /// </summary>
    /// <returns></returns>
    [HttpGet("/server/use")]
    public async Task<dynamic> GetServerUseInfo()
    {
        return await Task.FromResult(ServerUtil.GetServerUseInfo());
    }

    /// <summary>
    /// 服务器网络信息
    /// </summary>
    /// <returns></returns>
    [HttpGet("/server/network")]
    public async Task<dynamic> GetServerNetWorkInfo()
    {
        return await ServerUtil.GetServerNetWorkInfo();
    }
}