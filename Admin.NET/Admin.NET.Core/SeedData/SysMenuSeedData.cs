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
    public IEnumerable<SysMenu> HasData()
    {
        return new[]
        {
            new SysMenu{ Id=252885263002100, Pid=0, Title="数据面板", Path="/dashboard", Name="dashboard", Component="Layout", Redirect="/dashboard/workbench", Icon="ele-HomeFilled", Type=MenuTypeEnum.Dir, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=10 },
            new SysMenu{ Id=252885263002110, Pid=252885263002100, Title="工作台", Path="/dashboard/workbench", Name="workbench", Component="/dashboard/workbench/index", IsAffix=true, Icon="ele-DataAnalysis", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            // new SysMenu{ Id=252885263002111, Pid=252885263002100, Title="分析页", Path="/dashboard/analysis", Name="analysis", Component="/dashboard/analysis/index", Icon="ele-DataLine", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=101 },

            new SysMenu{ Id=252885263002200, Pid=0, Title="系统管理", Path="/system", Name="system", Component="Layout", Redirect="/system/user", Icon="ele-Setting", Type=MenuTypeEnum.Dir, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },

            new SysMenu{ Id=252885263002210, Pid=252885263002200, Title="账号管理", Path="/system/user", Name="sysUser", Component="/system/user/index", Icon="ele-User", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002211, Pid=252885263002210, Title="账号查询", Permission="sysUser:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002212, Pid=252885263002210, Title="账号编辑", Permission="sysUser:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002213, Pid=252885263002210, Title="账号增加", Permission="sysUser:add", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002214, Pid=252885263002210, Title="账号删除", Permission="sysUser:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002215, Pid=252885263002210, Title="授权角色", Permission="sysUser:grantRole", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002216, Pid=252885263002210, Title="授权数据", Permission="sysUser:grantData", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002217, Pid=252885263002210, Title="重置密码", Permission="sysUser:resetPwd", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002218, Pid=252885263002210, Title="设置状态", Permission="sysUser:setStatus", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },

            new SysMenu{ Id=252885263002220, Pid=252885263002200, Title="角色管理", Path="/system/role", Name="sysRole", Component="/system/role/index", Icon="ele-UserFilled", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=110 },
            new SysMenu{ Id=252885263002221, Pid=252885263002220, Title="角色查询", Permission="sysRole:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002222, Pid=252885263002220, Title="角色编辑", Permission="sysRole:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002223, Pid=252885263002220, Title="角色增加", Permission="sysRole:add", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002224, Pid=252885263002220, Title="角色删除", Permission="sysRole:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002225, Pid=252885263002220, Title="授权菜单", Permission="sysRole:grantMenu", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002226, Pid=252885263002220, Title="授权数据", Permission="sysRole:grantData", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002227, Pid=252885263002220, Title="设置状态", Permission="sysRole:setStatus", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },

            new SysMenu{ Id=252885263002230, Pid=252885263002200, Title="菜单管理", Path="/system/menu", Name="sysMenu", Component="/system/menu/index", Icon="ele-Menu", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=120 },
            new SysMenu{ Id=252885263002231, Pid=252885263002230, Title="菜单查询", Permission="sysMenu:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002232, Pid=252885263002230, Title="菜单编辑", Permission="sysMenu:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002233, Pid=252885263002230, Title="菜单增加", Permission="sysMenu:add", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002234, Pid=252885263002230, Title="菜单删除", Permission="sysMenu:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },

            new SysMenu{ Id=252885263002240, Pid=252885263002200, Title="机构管理", Path="/system/org", Name="sysOrg", Component="/system/org/index", Icon="ele-OfficeBuilding", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=130 },
            new SysMenu{ Id=252885263002241, Pid=252885263002240, Title="机构查询", Permission="sysOrg:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002242, Pid=252885263002240, Title="机构编辑", Permission="sysOrg:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002243, Pid=252885263002240, Title="机构增加", Permission="sysOrg:add", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002244, Pid=252885263002240, Title="机构删除", Permission="sysOrg:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },

            new SysMenu{ Id=252885263002250, Pid=252885263002200, Title="职位管理", Path="/system/pos", Name="sysPos", Component="/system/pos/index",Icon="ele-Mug", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=140 },
            new SysMenu{ Id=252885263002251, Pid=252885263002250, Title="职位查询", Permission="sysPos:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002252, Pid=252885263002250, Title="职位编辑", Permission="sysPos:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002253, Pid=252885263002250, Title="职位增加", Permission="sysPos:add", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002254, Pid=252885263002250, Title="职位删除", Permission="sysPos:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },

            new SysMenu{ Id=252885263002260, Pid=252885263002200, Title="修改密码", Path="/system/password", Name="sysPassword", Component="/system/password/index",Icon="ele-Hide", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=150 },

            new SysMenu{ Id=252885263002300, Pid=0, Title="平台管理", Path="/platform", Name="platform", Component="Layout", Redirect="/platform/config", Icon="ele-Menu", Type=MenuTypeEnum.Dir, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=200 },

            new SysMenu{ Id=252885263002310, Pid=252885263002300, Title="租户管理", Path="/platform/tenant", Name="sysTenant", Component="/system/tenant/index", Icon="ele-School", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002311, Pid=252885263002310, Title="租户查询", Permission="sysTenant:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002312, Pid=252885263002310, Title="租户编辑", Permission="sysTenant:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002313, Pid=252885263002310, Title="租户增加", Permission="sysTenant:add", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002314, Pid=252885263002310, Title="租户删除", Permission="sysTenant:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002315, Pid=252885263002310, Title="授权菜单", Permission="sysTenant:grantMenu", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002316, Pid=252885263002310, Title="重置密码", Permission="sysTenant:resetPwd", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },

            new SysMenu{ Id=252885263002320, Pid=252885263002300, Title="参数配置", Path="/platform/config", Name="sysConfig", Component="/system/config/index", Icon="ele-DocumentCopy", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=110 },
            new SysMenu{ Id=252885263002321, Pid=252885263002320, Title="配置查询", Permission="sysConfig:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002322, Pid=252885263002320, Title="配置编辑", Permission="sysConfig:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002323, Pid=252885263002320, Title="配置增加", Permission="sysConfig:add", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002324, Pid=252885263002320, Title="配置删除", Permission="sysConfig:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },

            new SysMenu{ Id=252885263002330, Pid=252885263002300, Title="字典管理", Path="/platform/dict", Name="sysDict", Component="/system/dict/index", Icon="ele-Collection", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=120 },
            new SysMenu{ Id=252885263002331, Pid=252885263002330, Title="字典查询", Permission="sysDict:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002332, Pid=252885263002330, Title="字典编辑", Permission="sysDict:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002333, Pid=252885263002330, Title="字典增加", Permission="sysDict:add", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002334, Pid=252885263002330, Title="字典删除", Permission="sysDict:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },

            new SysMenu{ Id=252885263002340, Pid=252885263002300, Title="任务调度", Path="/platform/timer", Name="sysTimer", Component="/system/timer/index", Icon="ele-AlarmClock", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=130 },
            new SysMenu{ Id=252885263002341, Pid=252885263002340, Title="任务查询", Permission="sysTimer:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002342, Pid=252885263002340, Title="任务编辑", Permission="sysTimer:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002343, Pid=252885263002340, Title="任务增加", Permission="sysTimer:add", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002344, Pid=252885263002340, Title="任务删除", Permission="sysTimer:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002345, Pid=252885263002340, Title="设置状态", Permission="sysTimer:setStatus", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },

            new SysMenu{ Id=252885263002350, Pid=252885263002300, Title="在线用户", Path="/platform/online", Name="sysOnline", Component="/system/online/index", Icon="ele-Sunny", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=140 },
            new SysMenu{ Id=252885263002351, Pid=252885263002350, Title="用户查询", Permission="online:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002352, Pid=252885263002350, Title="用户删除", Permission="online:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002353, Pid=252885263002350, Title="强制下线", Permission="online:ForceExistUser", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },

            new SysMenu{ Id=252885263002360, Pid=252885263002300, Title="系统监控", Path="/platform/server", Name="sysServer", Component="/system/server/index", Icon="ele-Monitor", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=150 },

            new SysMenu{ Id=252885263002370, Pid=252885263002300, Title="缓存管理", Path="/platform/cache", Name="sysCache", Component="/system/cache/index", Icon="ele-Loading", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=160 },
            new SysMenu{ Id=252885263002371, Pid=252885263002370, Title="缓存查询", Permission="cache:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002372, Pid=252885263002370, Title="缓存删除", Permission="cache:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },

            new SysMenu{ Id=252885263002380, Pid=252885263002300, Title="数据资源", Path="/platform/dataResource", Name="sysDataResource", Component="/system/dataResource/index", Icon="ele-TakeawayBox", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-05-30 00:00:00"), Order=170 },
            new SysMenu{ Id=252885263002381, Pid=252885263002380, Title="资源查询", Permission="sysDataResource:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-05-30 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002382, Pid=252885263002380, Title="资源编辑", Permission="sysDataResource:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-05-30 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002383, Pid=252885263002380, Title="资源增加", Permission="sysDataResource:add", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-05-30 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002384, Pid=252885263002380, Title="资源删除", Permission="sysDataResource:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-05-30 00:00:00"), Order=100 },

            new SysMenu{ Id=252885263002390, Pid=252885263002300, Title="文件管理", Path="/platform/file", Name="sysFile", Component="/system/file/index", Icon="ele-Document", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=180 },
            new SysMenu{ Id=252885263002391, Pid=252885263002390, Title="文件查询", Permission="sysFile:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002392, Pid=252885263002390, Title="文件上传", Permission="sysFile:upload", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002393, Pid=252885263002390, Title="文件下载", Permission="sysFile:download", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002394, Pid=252885263002390, Title="文件删除", Permission="sysFile:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },

            new SysMenu{ Id=252885263002500, Pid=0, Title="日志管理", Path="/log", Name="log", Component="Layout", Redirect="/log/vislog", Icon="ele-DocumentCopy", Type=MenuTypeEnum.Dir, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=300 },
            new SysMenu{ Id=252885263002510, Pid=252885263002500, Title="访问日志", Path="/log/vislog", Name="sysVislog", Component="/system/log/vislog/index", Icon="ele-Document", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002511, Pid=252885263002510, Title="日志查询", Permission="sysVislog:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002512, Pid=252885263002510, Title="日志清空", Permission="sysVislog:clear", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002520, Pid=252885263002500, Title="操作日志", Path="/log/oplog", Name="sysOplog", Component="/system/log/oplog/index", Icon="ele-Document", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=110 },
            new SysMenu{ Id=252885263002521, Pid=252885263002520, Title="日志查询", Permission="sysOplog:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002522, Pid=252885263002520, Title="日志清空", Permission="sysOplog:clear", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002530, Pid=252885263002500, Title="异常日志", Path="/log/exlog", Name="sysExlog", Component="/system/log/exlog/index", Icon="ele-Document", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=120 },
            new SysMenu{ Id=252885263002531, Pid=252885263002530, Title="日志查询", Permission="sysExlog:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002532, Pid=252885263002530, Title="日志清空", Permission="sysExlog:clear", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002540, Pid=252885263002500, Title="差异日志", Path="/log/difflog", Name="sysDifflog", Component="/system/log/difflog/index", Icon="ele-Document", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=130 },
            new SysMenu{ Id=252885263002541, Pid=252885263002540, Title="日志查询", Permission="sysDifflog:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002542, Pid=252885263002540, Title="日志清空", Permission="sysDifflog:clear", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },

            new SysMenu{ Id=252885263002600, Pid=0, Title="开发工具", Path="/develop", Name="develop", Component="Layout", Redirect="/develop/api", Icon="ele-Cpu", Type=MenuTypeEnum.Dir, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=400 },
            new SysMenu{ Id=252885263002610, Pid=252885263002600, Title="系统接口", Path="/develop/api", Name="sysApi", Component="layout/routerView/iframe", IsIframe=true, OutLink="https://localhost:44326/api/", Icon="ele-Help", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002611, Pid=252885263002600, Title="表单设计", Path="/develop/design", Name="sysDesign", Component="/system/code/index", Icon="ele-Edit", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=101 },
            new SysMenu{ Id=252885263002612, Pid=252885263002600, Title="代码生成", Path="/develop/code", Name="sysCode", Component="/system/code/index", Icon="ele-Crop", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=102 },
            new SysMenu{ Id=252885263002613, Pid=252885263002600, Title="库表管理", Path="/develop/database", Name="sysDatabase", Component="/system/database/index",Icon="ele-Coin", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=103 },

            new SysMenu{ Id=252885263002700, Pid=0, Title="帮助文档", Path="/doc", Name="doc", Component="Layout", Redirect="/doc/furion", Icon="ele-Notebook", Type=MenuTypeEnum.Dir, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=500 },
            new SysMenu{ Id=252885263002710, Pid=252885263002700, Title="后台教程", Path="/doc/furion", Name="sysFurion", Component="IFrame", IsIframe=false, IsKeepAlive=false, OutLink="https://furion.baiqian.ltd/", Icon="ele-Promotion", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=100 },
            new SysMenu{ Id=252885263002711, Pid=252885263002700, Title="前端教程", Path="/doc/element", Name="sysElement", Component="IFrame", IsIframe=false, IsKeepAlive=false, OutLink="https://element-plus.gitee.io/zh-CN/", Icon="ele-Position", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), Order=101 },
        };
    }
}