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
            new SysMenu{ Id=252885263003710, Pid=0, Title="数据面板", Path="/dashboard", Name="dashboard", Component="Layout", Redirect="/dashboard/workbench", Icon="ele-HomeFilled", Type=MenuTypeEnum.Dir, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=10 },
            new SysMenu{ Id=252885263003711, Pid=252885263003710, Title="工作台", Path="/dashboard/workbench", Name="workbench", Component="/dashboard/workbench/index", IsAffix=true, Icon="ele-DataAnalysis", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            //new SysMenu{ Id=252885263003712, Pid=252885263003710, Title="分析页", Path="/dashboard/analysis", Name="analysis", Component="/dashboard/analysis/index", Icon="ele-DataLine", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=101 },

            new SysMenu{ Id=252885263003720, Pid=0, Title="系统管理", Path="/system", Name="system", Component="Layout", Redirect="/system/user", Icon="ele-Setting", Type=MenuTypeEnum.Dir, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003730, Pid=252885263003720, Title="账号管理", Path="/system/user", Name="user", Component="/system/user/index", Icon="ele-User", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003731, Pid=252885263003730, Title="账号查询", Permission="sysUser:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003732, Pid=252885263003730, Title="账号编辑", Permission="sysUser:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003733, Pid=252885263003730, Title="账号增加", Permission="sysUser:add", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003734, Pid=252885263003730, Title="账号删除", Permission="sysUser:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003735, Pid=252885263003730, Title="授权角色", Permission="sysUser:grantRole", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003736, Pid=252885263003730, Title="授权数据", Permission="sysUser:grantData", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003737, Pid=252885263003730, Title="重置密码", Permission="sysUser:resetPwd", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003738, Pid=252885263003730, Title="设置状态", Permission="sysUser:setStatus", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003740, Pid=252885263003720, Title="角色管理", Path="/system/role", Name="role", Component="/system/role/index", Icon="ele-UserFilled", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=101 },
            new SysMenu{ Id=252885263003741, Pid=252885263003740, Title="角色查询", Permission="sysRole:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003742, Pid=252885263003740, Title="角色编辑", Permission="sysRole:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003743, Pid=252885263003740, Title="角色增加", Permission="sysRole:add", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003744, Pid=252885263003740, Title="角色删除", Permission="sysRole:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003745, Pid=252885263003740, Title="授权菜单", Permission="sysRole:grantMenu", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003746, Pid=252885263003740, Title="授权数据", Permission="sysRole:grantData", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003747, Pid=252885263003740, Title="设置状态", Permission="sysRole:setStatus", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003750, Pid=252885263003720, Title="菜单管理", Path="/system/menu", Name="menu", Component="/system/menu/index", Icon="ele-Menu", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=102 },
            new SysMenu{ Id=252885263003751, Pid=252885263003750, Title="菜单查询", Permission="sysMenu:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003752, Pid=252885263003750, Title="菜单编辑", Permission="sysMenu:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003753, Pid=252885263003750, Title="菜单增加", Permission="sysMenu:add", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003754, Pid=252885263003750, Title="菜单删除", Permission="sysMenu:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003760, Pid=252885263003720, Title="机构管理", Path="/system/org", Name="org", Component="/system/org/index", Icon="ele-OfficeBuilding", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=103 },
            new SysMenu{ Id=252885263003761, Pid=252885263003760, Title="机构查询", Permission="sysOrg:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003762, Pid=252885263003760, Title="机构编辑", Permission="sysOrg:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003763, Pid=252885263003760, Title="机构增加", Permission="sysOrg:add", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003764, Pid=252885263003760, Title="机构删除", Permission="sysOrg:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003770, Pid=252885263003720, Title="职位管理", Path="/system/pos", Name="pos", Component="/system/pos/index",Icon="ele-Mug", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=104 },
            new SysMenu{ Id=252885263003771, Pid=252885263003770, Title="职位查询", Permission="sysPos:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003772, Pid=252885263003770, Title="职位编辑", Permission="sysPos:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003773, Pid=252885263003770, Title="职位增加", Permission="sysPos:add", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003774, Pid=252885263003770, Title="职位删除", Permission="sysPos:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003775, Pid=252885263003720, Title="修改密码", Path="/system/password", Name="password", Component="/system/password/index",Icon="ele-Hide", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=105 },

            new SysMenu{ Id=252885263003780, Pid=0, Title="平台管理", Path="/platform", Name="platform", Component="Layout", Redirect="/platform/config", Icon="ele-Menu", Type=MenuTypeEnum.Dir, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=200 },
            new SysMenu{ Id=252885263003790, Pid=252885263003780, Title="租户管理", Path="/platform/tenant", Name="tenant", Component="/system/tenant/index", Icon="ele-School", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003791, Pid=252885263003790, Title="租户查询", Permission="sysTenant:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003792, Pid=252885263003790, Title="租户编辑", Permission="sysTenant:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003793, Pid=252885263003790, Title="租户增加", Permission="sysTenant:add", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003794, Pid=252885263003790, Title="租户删除", Permission="sysTenant:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003795, Pid=252885263003790, Title="授权菜单", Permission="sysTenant:grantMenu", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003796, Pid=252885263003790, Title="重置密码", Permission="sysTenant:resetPwd", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },

            new SysMenu{ Id=252885263003800, Pid=252885263003780, Title="参数配置", Path="/platform/config", Name="config", Component="/system/config/index", Icon="ele-DocumentCopy", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=101 },
            new SysMenu{ Id=252885263003801, Pid=252885263003800, Title="配置查询", Permission="sysConfig:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003802, Pid=252885263003800, Title="配置编辑", Permission="sysConfig:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003803, Pid=252885263003800, Title="配置增加", Permission="sysConfig:add", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003804, Pid=252885263003800, Title="配置删除", Permission="sysConfig:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },

            new SysMenu{ Id=252885263003810, Pid=252885263003780, Title="字典管理", Path="/platform/dict", Name="dict", Component="/system/dict/index", Icon="ele-Collection", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=102 },
            new SysMenu{ Id=252885263003811, Pid=252885263003810, Title="字典查询", Permission="sysDict:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003812, Pid=252885263003810, Title="字典编辑", Permission="sysDict:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003813, Pid=252885263003810, Title="字典增加", Permission="sysDict:add", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003814, Pid=252885263003810, Title="字典删除", Permission="sysDict:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },

            new SysMenu{ Id=252885263003820, Pid=252885263003780, Title="短信管理", Path="/platform/sms", Name="sms", Component="/system/sms/index", Icon="ele-Message", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=103 },

            new SysMenu{ Id=252885263003830, Pid=252885263003780, Title="任务调度", Path="/platform/timer", Name="timer", Component="/system/timer/index", Icon="ele-AlarmClock", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=104 },
            new SysMenu{ Id=252885263003831, Pid=252885263003830, Title="任务查询", Permission="sysTimer:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003832, Pid=252885263003830, Title="任务编辑", Permission="sysTimer:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003833, Pid=252885263003830, Title="任务增加", Permission="sysTimer:add", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003834, Pid=252885263003830, Title="任务删除", Permission="sysTimer:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003835, Pid=252885263003830, Title="设置状态", Permission="sysTimer:setStatus", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },

            new SysMenu{ Id=252885263003840, Pid=252885263003780, Title="代码生成", Path="/platform/code", Name="code", Component="/system/code/index", Icon="ele-Crop", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=105 },

            new SysMenu{ Id=252885263003850, Pid=252885263003780, Title="库表管理", Path="/platform/database", Name="database", Component="/system/database/index",Icon="ele-Coin", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=106 },

            new SysMenu{ Id=252885263003860, Pid=252885263003780, Title="在线用户", Path="/platform/online", Name="online", Component="/system/online/index", Icon="ele-Sunny", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=107 },
            new SysMenu{ Id=252885263003861, Pid=252885263003860, Title="用户查询", Permission="online:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003862, Pid=252885263003860, Title="用户删除", Permission="online:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003863, Pid=252885263003860, Title="强制下线", Permission="online:ForceExistUser", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },

            new SysMenu{ Id=252885263003870, Pid=252885263003780, Title="系统监控", Path="/platform/server", Name="server", Component="/system/server/index", Icon="ele-Monitor", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=108 },

            new SysMenu{ Id=252885263003880, Pid=252885263003780, Title="缓存管理", Path="/platform/cache", Name="cache", Component="/system/cache/index", Icon="ele-Loading", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=109 },
            new SysMenu{ Id=252885263003881, Pid=252885263003880, Title="缓存查询", Permission="cache:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003882, Pid=252885263003880, Title="缓存删除", Permission="cache:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },

            new SysMenu{ Id=252885263003890, Pid=252885263003780, Title="数据资源", Path="/platform/dataResource", Name="dataResource", Component="/system/dataResource/index", Icon="ele-TakeawayBox", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-05-30 00:00:00"), OrderNo=110 },
            new SysMenu{ Id=252885263003891, Pid=252885263003890, Title="资源查询", Permission="sysDataResource:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-05-30 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003892, Pid=252885263003890, Title="资源编辑", Permission="sysDataResource:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-05-30 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003893, Pid=252885263003890, Title="资源增加", Permission="sysDataResource:add", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-05-30 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003894, Pid=252885263003890, Title="资源删除", Permission="sysDataResource:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-05-30 00:00:00"), OrderNo=100 },

            new SysMenu{ Id=252885263003900, Pid=252885263003780, Title="文件管理", Path="/platform/file", Name="file", Component="/system/file/index", Icon="ele-Document", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=111 },
            new SysMenu{ Id=252885263003901, Pid=252885263003900, Title="文件查询", Permission="sysFile:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003902, Pid=252885263003900, Title="文件上传", Permission="sysFile:upload", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003903, Pid=252885263003900, Title="文件下载", Permission="sysFile:download", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263003904, Pid=252885263003900, Title="文件删除", Permission="sysFile:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },

            new SysMenu{ Id=252885263004000, Pid=0, Title="日志管理", Path="/log", Name="log", Component="Layout", Redirect="/log/vislog", Icon="ele-DocumentCopy", Type=MenuTypeEnum.Dir, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=200 },
            new SysMenu{ Id=252885263004010, Pid=252885263004000, Title="访问日志", Path="/log/vislog", Name="vislog", Component="/system/log/vislog/index", Icon="ele-Document", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263004011, Pid=252885263004010, Title="日志查询", Permission="sysVislog:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263004012, Pid=252885263004010, Title="日志清空", Permission="sysVislog:clear", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263004020, Pid=252885263004000, Title="操作日志", Path="/log/oplog", Name="oplog", Component="/system/log/oplog/index", Icon="ele-Document", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=101 },
            new SysMenu{ Id=252885263004021, Pid=252885263004020, Title="日志查询", Permission="sysOplog:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263004022, Pid=252885263004020, Title="日志清空", Permission="sysOplog:clear", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263004030, Pid=252885263004000, Title="异常日志", Path="/log/exlog", Name="exlog", Component="/system/log/exlog/index", Icon="ele-Document", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=102 },
            new SysMenu{ Id=252885263004031, Pid=252885263004030, Title="日志查询", Permission="sysExlog:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263004032, Pid=252885263004030, Title="日志清空", Permission="sysExlog:clear", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263004035, Pid=252885263004000, Title="差异日志", Path="/log/difflog", Name="difflog", Component="/system/log/difflog/index", Icon="ele-Document", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263004036, Pid=252885263004035, Title="日志查询", Permission="sysDifflog:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263004037, Pid=252885263004035, Title="日志清空", Permission="sysDifflog:clear", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },

            new SysMenu{ Id=252885263004100, Pid=0, Title="帮助文档", Path="/doc", Name="doc", Component="Layout", Redirect="/doc/api", Icon="ele-Notebook", Type=MenuTypeEnum.Dir, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=300 },
            new SysMenu{ Id=252885263004101, Pid=252885263004100, Title="接口文档", Path="/doc/api", Name="api", Component="layout/routerView/iframe", IsIframe=true, OutLink="https://localhost:44326/api/", Icon="ele-Help", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263004102, Pid=252885263004100, Title="后台文档", Path="/doc/furion", Name="furion", Component="IFrame", IsIframe=false, IsKeepAlive=false, OutLink="https://furion.baiqian.ltd/", Icon="ele-Promotion", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=252885263004103, Pid=252885263004100, Title="前端文档", Path="/doc/element", Name="element", Component="IFrame", IsIframe=false, IsKeepAlive=false, OutLink="https://element-plus.gitee.io/zh-CN/", Icon="ele-Position", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
        };
    }
}