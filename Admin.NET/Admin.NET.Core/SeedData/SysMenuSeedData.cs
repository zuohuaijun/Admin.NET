namespace Admin.NET.Core;

/// <summary>
/// 系统菜单表种子数据
/// </summary>
public class SysMenuSeedData : ISqlSugarEntitySeedData<SysMenu>
{
    /// <summary>
    /// 种子数据
    /// </summary>
    /// <returns></returns>
    [IgnoreUpdate]
    public IEnumerable<SysMenu> HasData()
    {
        return new[]
        {
            new SysMenu{ Id=252885263002100, Pid=0, Title="数据面板", Path="/dashboard", Name="dashboard", Component="Layout", Redirect="/dashboard/home", Icon="ele-HomeFilled", Type=MenuTypeEnum.Dir, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=10 },
            new SysMenu{ Id=252885263002110, Pid=252885263002100, Title="工作台", Path="/dashboard/home", Name="home", Component="/home/index", IsAffix=true, Icon="ele-HomeFilled", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002111, Pid=252885263002100, Title="站内信", Path="/dashboard/notice", Name="notice", Component="/home/notice/index", Icon="ele-Bell", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=101 },

            new SysMenu{ Id=252885263005200, Pid=0, Title="系统管理", Path="/system", Name="system", Component="Layout", Redirect="/system/user", Icon="ele-Setting", Type=MenuTypeEnum.Dir, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },

            new SysMenu{ Id=252885263005210, Pid=252885263005200, Title="账号管理", Path="/system/user", Name="sysUser", Component="/system/user/index", Icon="ele-User", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005211, Pid=252885263005210, Title="查询", Permission="sysUser:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005212, Pid=252885263005210, Title="编辑", Permission="sysUser:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005213, Pid=252885263005210, Title="增加", Permission="sysUser:add", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005214, Pid=252885263005210, Title="删除", Permission="sysUser:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005215, Pid=252885263005210, Title="详情", Permission="sysUser:detail", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005216, Pid=252885263005210, Title="授权角色", Permission="sysUser:grantRole", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005217, Pid=252885263005210, Title="重置密码", Permission="sysUser:resetPwd", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005218, Pid=252885263005210, Title="设置状态", Permission="sysUser:setStatus", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005219, Pid=252885263005210, Title="修改密码", Permission="sysUser:changePwd", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005220, Pid=252885263005210, Title="强制下线", Permission="sysUser:forceOffline", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },

            new SysMenu{ Id=252885263005230, Pid=252885263005200, Title="角色管理", Path="/system/role", Name="sysRole", Component="/system/role/index", Icon="ele-Help", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=110 },
            new SysMenu{ Id=252885263005231, Pid=252885263005230, Title="查询", Permission="sysRole:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005232, Pid=252885263005230, Title="编辑", Permission="sysRole:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005233, Pid=252885263005230, Title="增加", Permission="sysRole:add", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005234, Pid=252885263005230, Title="删除", Permission="sysRole:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005235, Pid=252885263005230, Title="授权菜单", Permission="sysRole:grantMenu", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005236, Pid=252885263005230, Title="授权数据", Permission="sysRole:grantData", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005237, Pid=252885263005230, Title="设置状态", Permission="sysRole:setStatus", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },

            new SysMenu{ Id=252885263005240, Pid=252885263005200, Title="机构管理", Path="/system/org", Name="sysOrg", Component="/system/org/index", Icon="ele-OfficeBuilding", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=130 },
            new SysMenu{ Id=252885263005241, Pid=252885263005240, Title="查询", Permission="sysOrg:list", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005242, Pid=252885263005240, Title="编辑", Permission="sysOrg:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005243, Pid=252885263005240, Title="增加", Permission="sysOrg:add", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005244, Pid=252885263005240, Title="删除", Permission="sysOrg:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },

            new SysMenu{ Id=252885263005250, Pid=252885263005200, Title="职位管理", Path="/system/pos", Name="sysPos", Component="/system/pos/index",Icon="ele-Mug", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=140 },
            new SysMenu{ Id=252885263005251, Pid=252885263005250, Title="查询", Permission="sysPos:list", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005252, Pid=252885263005250, Title="编辑", Permission="sysPos:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005253, Pid=252885263005250, Title="增加", Permission="sysPos:add", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005254, Pid=252885263005250, Title="删除", Permission="sysPos:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },

            new SysMenu{ Id=252885263005260, Pid=252885263005200, Title="个人中心", Path="/system/userCenter", Name="sysUserCenter", Component="/system/user/component/userCenter",Icon="ele-Medal", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=150 },
            new SysMenu{ Id=252885263005261, Pid=252885263005260, Title="修改密码", Permission="sysUser:changePwd", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005262, Pid=252885263005260, Title="更新信息", Permission="sysUser:updateBase", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005263, Pid=252885263005260, Title="电子签名", Permission="sysUser:signature", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },

            new SysMenu{ Id=252885263005270, Pid=252885263005200, Title="通知公告", Path="/system/notice", Name="sysNotice", Component="/system/notice/index",Icon="ele-Bell", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=160 },
            new SysMenu{ Id=252885263005271, Pid=252885263005270, Title="查询", Permission="sysNotice:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005272, Pid=252885263005270, Title="编辑", Permission="sysNotice:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005273, Pid=252885263005270, Title="增加", Permission="sysNotice:add", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005274, Pid=252885263005270, Title="删除", Permission="sysNotice:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005275, Pid=252885263005270, Title="发布", Permission="sysNotice:public", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005276, Pid=252885263005270, Title="撤回", Permission="sysNotice:cancel", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
               
            new SysMenu{ Id=252885263005280, Pid=252885263005200, Title="三方账号", Path="/system/weChatUser", Name="weChatUser", Component="/system/weChatUser/index",Icon="ele-ChatDotRound", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=170 },
            new SysMenu{ Id=252885263005281, Pid=252885263005280, Title="查询", Permission="weChatUser:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005282, Pid=252885263005280, Title="编辑", Permission="weChatUser:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005283, Pid=252885263005280, Title="删除", Permission="weChatUser:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },

            new SysMenu{ Id=252885263005300, Pid=0, Title="平台管理", Path="/platform", Name="platform", Component="Layout", Redirect="/platform/tenant", Icon="ele-Menu", Type=MenuTypeEnum.Dir, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=200 },

            new SysMenu{ Id=252885263005310, Pid=252885263005300, Title="租户管理", Path="/platform/tenant", Name="sysTenant", Component="/system/tenant/index", Icon="ele-School", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005311, Pid=252885263005310, Title="查询", Permission="sysTenant:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005312, Pid=252885263005310, Title="编辑", Permission="sysTenant:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005313, Pid=252885263005310, Title="增加", Permission="sysTenant:add", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005314, Pid=252885263005310, Title="删除", Permission="sysTenant:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005315, Pid=252885263005310, Title="授权菜单", Permission="sysTenant:grantMenu", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005316, Pid=252885263005310, Title="重置密码", Permission="sysTenant:resetPwd", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005317, Pid=252885263005310, Title="生成库", Permission="sysTenant:createDb", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },

            new SysMenu{ Id=252885263005320, Pid=252885263005300, Title="菜单管理", Path="/platform/menu", Name="sysMenu", Component="/system/menu/index", Icon="ele-Menu", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=120 },
            new SysMenu{ Id=252885263005321, Pid=252885263005320, Title="查询", Permission="sysMenu:list", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005322, Pid=252885263005320, Title="编辑", Permission="sysMenu:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005323, Pid=252885263005320, Title="增加", Permission="sysMenu:add", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005324, Pid=252885263005320, Title="删除", Permission="sysMenu:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },

            new SysMenu{ Id=252885263005330, Pid=252885263005300, Title="参数配置", Path="/platform/config", Name="sysConfig", Component="/system/config/index", Icon="ele-DocumentCopy", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=110 },
            new SysMenu{ Id=252885263005331, Pid=252885263005330, Title="查询", Permission="sysConfig:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005332, Pid=252885263005330, Title="编辑", Permission="sysConfig:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005333, Pid=252885263005330, Title="增加", Permission="sysConfig:add", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005334, Pid=252885263005330, Title="删除", Permission="sysConfig:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },

            new SysMenu{ Id=252885263005340, Pid=252885263005300, Title="字典管理", Path="/platform/dict", Name="sysDict", Component="/system/dict/index", Icon="ele-Collection", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=120 },
            new SysMenu{ Id=252885263005341, Pid=252885263005340, Title="查询", Permission="sysDictType:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005342, Pid=252885263005340, Title="编辑", Permission="sysDictType:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005343, Pid=252885263005340, Title="增加", Permission="sysDictType:add", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005344, Pid=252885263005340, Title="删除", Permission="sysDictType:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },

            new SysMenu{ Id=252885263005350, Pid=252885263005300, Title="任务调度", Path="/platform/job", Name="sysJob", Component="/system/job/index", Icon="ele-AlarmClock", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=130 },
            new SysMenu{ Id=252885263005351, Pid=252885263005350, Title="查询", Permission="sysJob:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005352, Pid=252885263005350, Title="编辑", Permission="sysJob:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005353, Pid=252885263005350, Title="增加", Permission="sysJob:add", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005354, Pid=252885263005350, Title="删除", Permission="sysJob:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },

            new SysMenu{ Id=252885263005360, Pid=252885263005300, Title="系统监控", Path="/platform/server", Name="sysServer", Component="/system/server/index", Icon="ele-Monitor", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=140 },

            new SysMenu{ Id=252885263005370, Pid=252885263005300, Title="缓存管理", Path="/platform/cache", Name="sysCache", Component="/system/cache/index", Icon="ele-Loading", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=150 },
            new SysMenu{ Id=252885263005371, Pid=252885263005370, Title="查询", Permission="sysCache:keyList", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005372, Pid=252885263005370, Title="删除", Permission="sysCache:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },

            new SysMenu{ Id=252885263005380, Pid=252885263005300, Title="行政区域", Path="/platform/region", Name="sysRegion", Component="/system/region/index", Icon="ele-LocationInformation", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=160 },
            new SysMenu{ Id=252885263005381, Pid=252885263005380, Title="查询", Permission="sysRegion:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005382, Pid=252885263005380, Title="编辑", Permission="sysRegion:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005383, Pid=252885263005380, Title="增加", Permission="sysRegion:add", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005384, Pid=252885263005380, Title="删除", Permission="sysRegion:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005385, Pid=252885263005380, Title="同步", Permission="sysRegion:sync", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },

            new SysMenu{ Id=252885263005390, Pid=252885263005300, Title="文件管理", Path="/platform/file", Name="sysFile", Component="/system/file/index", Icon="ele-Document", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=170 },
            new SysMenu{ Id=252885263005391, Pid=252885263005390, Title="查询", Permission="sysFile:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005392, Pid=252885263005390, Title="上传", Permission="sysFile:upload", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005393, Pid=252885263005390, Title="下载", Permission="sysFile:download", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005394, Pid=252885263005390, Title="删除", Permission="sysFile:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },

            new SysMenu{ Id=252885263005500, Pid=0, Title="日志管理", Path="/log", Name="log", Component="Layout", Redirect="/log/vislog", Icon="ele-DocumentCopy", Type=MenuTypeEnum.Dir, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=300 },
            new SysMenu{ Id=252885263005510, Pid=252885263005500, Title="访问日志", Path="/log/vislog", Name="sysVislog", Component="/system/log/vislog/index", Icon="ele-Document", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005511, Pid=252885263005510, Title="查询", Permission="sysVislog:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005512, Pid=252885263005510, Title="清空", Permission="sysVislog:clear", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005520, Pid=252885263005500, Title="操作日志", Path="/log/oplog", Name="sysOplog", Component="/system/log/oplog/index", Icon="ele-Document", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=110 },
            new SysMenu{ Id=252885263005521, Pid=252885263005520, Title="查询", Permission="sysOplog:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005522, Pid=252885263005520, Title="清空", Permission="sysOplog:clear", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005523, Pid=252885263005520, Title="导出", Permission="sysOplog:export", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005530, Pid=252885263005500, Title="异常日志", Path="/log/exlog", Name="sysExlog", Component="/system/log/exlog/index", Icon="ele-Document", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=120 },
            new SysMenu{ Id=252885263005531, Pid=252885263005530, Title="查询", Permission="sysExlog:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005532, Pid=252885263005530, Title="清空", Permission="sysExlog:clear", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005533, Pid=252885263005530, Title="导出", Permission="sysExlog:export", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005540, Pid=252885263005500, Title="差异日志", Path="/log/difflog", Name="sysDifflog", Component="/system/log/difflog/index", Icon="ele-Document", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=130 },
            new SysMenu{ Id=252885263005541, Pid=252885263005540, Title="查询", Permission="sysDifflog:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005542, Pid=252885263005540, Title="清空", Permission="sysDifflog:clear", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },

            new SysMenu{ Id=252885263005600, Pid=0, Title="开发工具", Path="/develop", Name="develop", Component="Layout", Redirect="/develop/database", Icon="ele-Cpu", Type=MenuTypeEnum.Dir, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=400 },
            new SysMenu{ Id=252885263005610, Pid=252885263005600, Title="库表管理", Path="/develop/database", Name="sysDatabase", Component="/system/database/index",Icon="ele-Coin", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005620, Pid=252885263005600, Title="表单设计", Path="/develop/formDes", Name="sysFormDes", Component="/system/formDes/index", Icon="ele-Edit", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=101 },
            new SysMenu{ Id=252885263005630, Pid=252885263005600, Title="代码生成", Path="/develop/codeGen", Name="sysCodeGen", Component="/system/codeGen/index", Icon="ele-Crop", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=102 },
            new SysMenu{ Id=252885263005640, Pid=252885263005600, Title="系统接口", Path="/develop/api", Name="sysApi", Component="layout/routerView/iframe", IsIframe=true, OutLink="https://localhost:44326/api/", Icon="ele-Help", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=103 },

            new SysMenu{ Id=252885263005700, Pid=0, Title="帮助文档", Path="/doc", Name="doc", Component="Layout", Redirect="/doc/furion", Icon="ele-Notebook", Type=MenuTypeEnum.Dir, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=500 },
            new SysMenu{ Id=252885263005710, Pid=252885263005700, Title="后台教程", Path="/doc/furion", Name="sysFurion", Component="layout/routerView/link", IsIframe=false, IsKeepAlive=false, OutLink="https://furion.baiqian.ltd/", Icon="ele-Promotion", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263005711, Pid=252885263005700, Title="前端教程", Path="/doc/element", Name="sysElement", Component="layout/routerView/link", IsIframe=false, IsKeepAlive=false, OutLink="https://element-plus.gitee.io/zh-CN/", Icon="ele-Position", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=101 },
        };
    }
}