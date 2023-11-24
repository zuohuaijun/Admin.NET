// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

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
            new SysMenu{ Id=1300000000101, Pid=0, Title="工作台", Path="/dashboard", Name="dashboard", Component="Layout", Icon="ele-HomeFilled", Type=MenuTypeEnum.Dir, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=0 },
            new SysMenu{ Id=1300000000111, Pid=1300000000101, Title="工作台", Path="/dashboard/home", Name="home", Component="/home/index", IsAffix=true, Icon="ele-HomeFilled", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1300000000121, Pid=1300000000101, Title="站内信", Path="/dashboard/notice", Name="notice", Component="/home/notice/index", Icon="ele-Bell", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=101 },

            // 建议此处Id范围之间放置具体业务应用菜单

            new SysMenu{ Id=1310000000101, Pid=0, Title="系统管理", Path="/system", Name="system", Component="Layout", Icon="ele-Setting", Type=MenuTypeEnum.Dir, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=10000 },

            new SysMenu{ Id=1310000000111, Pid=1310000000101, Title="账号管理", Path="/system/user", Name="sysUser", Component="/system/user/index", Icon="ele-User", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000112, Pid=1310000000111, Title="查询", Permission="sysUser:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000113, Pid=1310000000111, Title="编辑", Permission="sysUser:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000114, Pid=1310000000111, Title="增加", Permission="sysUser:add", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000115, Pid=1310000000111, Title="删除", Permission="sysUser:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000116, Pid=1310000000111, Title="详情", Permission="sysUser:detail", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000117, Pid=1310000000111, Title="授权角色", Permission="sysUser:grantRole", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000118, Pid=1310000000111, Title="重置密码", Permission="sysUser:resetPwd", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000119, Pid=1310000000111, Title="设置状态", Permission="sysUser:setStatus", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000120, Pid=1310000000111, Title="强制下线", Permission="sysOnlineUser:forceOffline", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },

            new SysMenu{ Id=1310000000131, Pid=1310000000101, Title="角色管理", Path="/system/role", Name="sysRole", Component="/system/role/index", Icon="ele-Help", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=110 },
            new SysMenu{ Id=1310000000132, Pid=1310000000131, Title="查询", Permission="sysRole:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000133, Pid=1310000000131, Title="编辑", Permission="sysRole:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000134, Pid=1310000000131, Title="增加", Permission="sysRole:add", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000135, Pid=1310000000131, Title="删除", Permission="sysRole:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000136, Pid=1310000000131, Title="授权菜单", Permission="sysRole:grantMenu", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000137, Pid=1310000000131, Title="授权数据", Permission="sysRole:grantDataScope", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000138, Pid=1310000000131, Title="设置状态", Permission="sysRole:setStatus", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },

            new SysMenu{ Id=1310000000141, Pid=1310000000101, Title="机构管理", Path="/system/org", Name="sysOrg", Component="/system/org/index", Icon="ele-OfficeBuilding", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=120 },
            //new SysMenu{ Id=1310000000142, Pid=1310000000141, Title="查询", Permission="sysOrg:list", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000143, Pid=1310000000141, Title="编辑", Permission="sysOrg:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000144, Pid=1310000000141, Title="增加", Permission="sysOrg:add", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000145, Pid=1310000000141, Title="删除", Permission="sysOrg:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },

            new SysMenu{ Id=1310000000151, Pid=1310000000101, Title="职位管理", Path="/system/pos", Name="sysPos", Component="/system/pos/index",Icon="ele-Mug", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=130 },
            new SysMenu{ Id=1310000000152, Pid=1310000000151, Title="查询", Permission="sysPos:list", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000153, Pid=1310000000151, Title="编辑", Permission="sysPos:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000154, Pid=1310000000151, Title="增加", Permission="sysPos:add", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000155, Pid=1310000000151, Title="删除", Permission="sysPos:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },

            new SysMenu{ Id=1310000000161, Pid=1310000000101, Title="个人中心", Path="/system/userCenter", Name="sysUserCenter", Component="/system/user/component/userCenter",Icon="ele-Medal", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=140 },
            new SysMenu{ Id=1310000000162, Pid=1310000000161, Title="修改密码", Permission="sysUser:changePwd", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000163, Pid=1310000000161, Title="基本信息", Permission="sysUser:baseInfo", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000164, Pid=1310000000161, Title="电子签名", Permission="sysFile:uploadSignature", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000165, Pid=1310000000161, Title="上传头像", Permission="sysFile:uploadAvatar", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },

            new SysMenu{ Id=1310000000171, Pid=1310000000101, Title="通知公告", Path="/system/notice", Name="sysNotice", Component="/system/notice/index",Icon="ele-Bell", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=150 },
            new SysMenu{ Id=1310000000172, Pid=1310000000171, Title="查询", Permission="sysNotice:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000173, Pid=1310000000171, Title="编辑", Permission="sysNotice:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000174, Pid=1310000000171, Title="增加", Permission="sysNotice:add", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000175, Pid=1310000000171, Title="删除", Permission="sysNotice:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000176, Pid=1310000000171, Title="发布", Permission="sysNotice:public", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000177, Pid=1310000000171, Title="撤回", Permission="sysNotice:cancel", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },

            new SysMenu{ Id=1310000000181, Pid=1310000000101, Title="三方账号", Path="/system/weChatUser", Name="weChatUser", Component="/system/weChatUser/index",Icon="ele-ChatDotRound", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=160 },
            new SysMenu{ Id=1310000000182, Pid=1310000000181, Title="查询", Permission="sysWechatUser:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000183, Pid=1310000000181, Title="编辑", Permission="sysWechatUser:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000184, Pid=1310000000181, Title="增加", Permission="sysWechatUser:add", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000185, Pid=1310000000181, Title="删除", Permission="sysWechatUser:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },

            new SysMenu{ Id=1310000000301, Pid=0, Title="平台管理", Path="/platform", Name="platform", Component="Layout", Icon="ele-Menu", Type=MenuTypeEnum.Dir, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=11000 },

            new SysMenu{ Id=1310000000311, Pid=1310000000301, Title="租户管理", Path="/platform/tenant", Name="sysTenant", Component="/system/tenant/index", Icon="ele-School", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000312, Pid=1310000000311, Title="查询", Permission="sysTenant:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000313, Pid=1310000000311, Title="编辑", Permission="sysTenant:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000314, Pid=1310000000311, Title="增加", Permission="sysTenant:add", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000315, Pid=1310000000311, Title="删除", Permission="sysTenant:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000316, Pid=1310000000311, Title="授权菜单", Permission="sysTenant:grantMenu", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000317, Pid=1310000000311, Title="重置密码", Permission="sysTenant:resetPwd", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000318, Pid=1310000000311, Title="生成库", Permission="sysTenant:createDb", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000319, Pid=1310000000311, Title="设置状态", Permission="sysTenant:setStatus", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },

            new SysMenu{ Id=1310000000321, Pid=1310000000301, Title="菜单管理", Path="/platform/menu", Name="sysMenu", Component="/system/menu/index", Icon="ele-Menu", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=110 },
            new SysMenu{ Id=1310000000322, Pid=1310000000321, Title="查询", Permission="sysMenu:list", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000323, Pid=1310000000321, Title="编辑", Permission="sysMenu:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000324, Pid=1310000000321, Title="增加", Permission="sysMenu:add", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000325, Pid=1310000000321, Title="删除", Permission="sysMenu:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },

            new SysMenu{ Id=1310000000331, Pid=1310000000301, Title="参数配置", Path="/platform/config", Name="sysConfig", Component="/system/config/index", Icon="ele-DocumentCopy", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=120 },
            new SysMenu{ Id=1310000000332, Pid=1310000000331, Title="查询", Permission="sysConfig:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000333, Pid=1310000000331, Title="编辑", Permission="sysConfig:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000334, Pid=1310000000331, Title="增加", Permission="sysConfig:add", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000335, Pid=1310000000331, Title="删除", Permission="sysConfig:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },

            new SysMenu{ Id=1310000000341, Pid=1310000000301, Title="字典管理", Path="/platform/dict", Name="sysDict", Component="/system/dict/index", Icon="ele-Collection", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=130 },
            new SysMenu{ Id=1310000000342, Pid=1310000000341, Title="查询", Permission="sysDictType:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000343, Pid=1310000000341, Title="编辑", Permission="sysDictType:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000344, Pid=1310000000341, Title="增加", Permission="sysDictType:add", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000345, Pid=1310000000341, Title="删除", Permission="sysDictType:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },

            new SysMenu{ Id=1310000000351, Pid=1310000000301, Title="任务调度", Path="/platform/job", Name="sysJob", Component="/system/job/index", Icon="ele-AlarmClock", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=140 },
            new SysMenu{ Id=1310000000352, Pid=1310000000351, Title="查询", Permission="sysJob:pageJobDetail", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000353, Pid=1310000000351, Title="编辑", Permission="sysJob:updateJobDetail", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000354, Pid=1310000000351, Title="增加", Permission="sysJob:addJobDetail", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000355, Pid=1310000000351, Title="删除", Permission="sysJob:deleteJobDetail", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },

            new SysMenu{ Id=1310000000361, Pid=1310000000301, Title="系统监控", Path="/platform/server", Name="sysServer", Component="/system/server/index", Icon="ele-Monitor", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=150 },

            new SysMenu{ Id=1310000000371, Pid=1310000000301, Title="缓存管理", Path="/platform/cache", Name="sysCache", Component="/system/cache/index", Icon="ele-Loading", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=160 },
            new SysMenu{ Id=1310000000372, Pid=1310000000371, Title="查询", Permission="sysCache:keyList", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000373, Pid=1310000000371, Title="删除", Permission="sysCache:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },

            new SysMenu{ Id=1310000000381, Pid=1310000000301, Title="行政区域", Path="/platform/region", Name="sysRegion", Component="/system/region/index", Icon="ele-LocationInformation", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=170 },
            new SysMenu{ Id=1310000000382, Pid=1310000000381, Title="查询", Permission="sysRegion:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000383, Pid=1310000000381, Title="编辑", Permission="sysRegion:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000384, Pid=1310000000381, Title="增加", Permission="sysRegion:add", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000385, Pid=1310000000381, Title="删除", Permission="sysRegion:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000386, Pid=1310000000381, Title="同步", Permission="sysRegion:sync", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },

            new SysMenu{ Id=1310000000391, Pid=1310000000301, Title="文件管理", Path="/platform/file", Name="sysFile", Component="/system/file/index", Icon="ele-Document", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=180 },
            new SysMenu{ Id=1310000000392, Pid=1310000000391, Title="查询", Permission="sysFile:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000393, Pid=1310000000391, Title="上传", Permission="sysFile:uploadFile", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000394, Pid=1310000000391, Title="下载", Permission="sysFile:downloadFile", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000395, Pid=1310000000391, Title="删除", Permission="sysFile:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000396, Pid=1310000000391, Title="编辑", Permission="sysFile:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2023-10-27 00:00:00"), OrderNo=100 },

            new SysMenu{ Id=1310000000401, Pid=1310000000301, Title="打印模板", Path="/platform/print", Name="sysPrint", Component="/system/print/index", Icon="ele-Printer", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=190 },
            new SysMenu{ Id=1310000000402, Pid=1310000000401, Title="查询", Permission="sysPrint:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000403, Pid=1310000000401, Title="编辑", Permission="sysPrint:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000404, Pid=1310000000401, Title="增加", Permission="sysPrint:add", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000405, Pid=1310000000401, Title="删除", Permission="sysPrint:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },

            new SysMenu{ Id=1310000000411, Pid=1310000000301, Title="动态插件", Path="/platform/plugin", Name="sysPlugin", Component="/system/plugin/index", Icon="ele-Connection", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=200 },
            new SysMenu{ Id=1310000000412, Pid=1310000000411, Title="查询", Permission="sysPlugin:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000413, Pid=1310000000411, Title="编辑", Permission="sysPlugin:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000414, Pid=1310000000411, Title="增加", Permission="sysPlugin:add", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000415, Pid=1310000000411, Title="删除", Permission="sysPlugin:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },

            new SysMenu{ Id=1310000000421, Pid=1310000000301, Title="开放接口身份", Path="/platform/openAccess", Name="sysOpenAccess", Component="/system/openAccess/index", Icon="ele-Link", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=210 },
            new SysMenu{ Id=1310000000422, Pid=1310000000421, Title="查询", Permission="sysOpenAccess:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000423, Pid=1310000000421, Title="编辑", Permission="sysOpenAccess:update", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000424, Pid=1310000000421, Title="增加", Permission="sysOpenAccess:add", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000425, Pid=1310000000421, Title="删除", Permission="sysOpenAccess:delete", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },

            new SysMenu{ Id=1310000000501, Pid=0, Title="日志管理", Path="/log", Name="log", Component="Layout", Icon="ele-DocumentCopy", Type=MenuTypeEnum.Dir, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=12000 },
            new SysMenu{ Id=1310000000511, Pid=1310000000501, Title="访问日志", Path="/log/vislog", Name="sysVisLog", Component="/system/log/vislog/index", Icon="ele-Document", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000512, Pid=1310000000511, Title="查询", Permission="sysVislog:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000513, Pid=1310000000511, Title="清空", Permission="sysVislog:clear", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000521, Pid=1310000000501, Title="操作日志", Path="/log/oplog", Name="sysOpLog", Component="/system/log/oplog/index", Icon="ele-Document", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=110 },
            new SysMenu{ Id=1310000000522, Pid=1310000000521, Title="查询", Permission="sysOplog:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000523, Pid=1310000000521, Title="清空", Permission="sysOplog:clear", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000524, Pid=1310000000521, Title="导出", Permission="sysOplog:export", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000531, Pid=1310000000501, Title="异常日志", Path="/log/exlog", Name="sysExLog", Component="/system/log/exlog/index", Icon="ele-Document", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=120 },
            new SysMenu{ Id=1310000000532, Pid=1310000000531, Title="查询", Permission="sysExlog:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000533, Pid=1310000000531, Title="清空", Permission="sysExlog:clear", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000534, Pid=1310000000531, Title="导出", Permission="sysExlog:export", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000541, Pid=1310000000501, Title="差异日志", Path="/log/difflog", Name="sysDiffLog", Component="/system/log/difflog/index", Icon="ele-Document", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=130 },
            new SysMenu{ Id=1310000000542, Pid=1310000000541, Title="查询", Permission="sysDifflog:page", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000543, Pid=1310000000541, Title="清空", Permission="sysDifflog:clear", Type=MenuTypeEnum.Btn, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },

            new SysMenu{ Id=1310000000601, Pid=0, Title="开发工具", Path="/develop", Name="develop", Component="Layout", Icon="ele-Cpu", Type=MenuTypeEnum.Dir, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=13000 },
            new SysMenu{ Id=1310000000611, Pid=1310000000601, Title="库表管理", Path="/develop/database", Name="sysDatabase", Component="/system/database/index",Icon="ele-Coin", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000621, Pid=1310000000601, Title="代码生成", Path="/develop/codeGen", Name="sysCodeGen", Component="/system/codeGen/index", Icon="ele-Crop", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=110 },
            new SysMenu{ Id=1310000000631, Pid=1310000000601, Title="表单设计", Path="/develop/formDes", Name="sysFormDes", Component="/system/formDes/index", Icon="ele-Edit", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=120 },
            new SysMenu{ Id=1310000000641, Pid=1310000000601, Title="系统接口", Path="/develop/api", Name="sysApi", Component="layout/routerView/iframe", IsIframe=true, OutLink="http://localhost:5005", Icon="ele-Help", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=130 },

            new SysMenu{ Id=1310000000701, Pid=0, Title="帮助文档", Path="/doc", Name="doc", Component="Layout", Icon="ele-Notebook", Type=MenuTypeEnum.Dir, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=14000 },
            new SysMenu{ Id=1310000000711, Pid=1310000000701, Title="后台教程", Path="/doc/furion", Name="sysFurion", Component="layout/routerView/link", IsIframe=false, IsKeepAlive=false, OutLink="https://furion.baiqian.ltd/", Icon="ele-Promotion", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=100 },
            new SysMenu{ Id=1310000000712, Pid=1310000000701, Title="前端教程", Path="/doc/element", Name="sysElement", Component="layout/routerView/link", IsIframe=false, IsKeepAlive=false, OutLink="https://element-plus.gitee.io/zh-CN/", Icon="ele-Position", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=110 },
            new SysMenu{ Id=1310000000713, Pid=1310000000701, Title="SqlSugar", Path="/doc/SqlSugar", Name="sysSqlSugar", Component="layout/routerView/link", IsIframe=false, IsKeepAlive=false, OutLink="https://www.donet5.com/Home/Doc", Icon="ele-Coin", Type=MenuTypeEnum.Menu, CreateTime=DateTime.Parse("2022-02-10 00:00:00"), OrderNo=120 },
        };
    }
}