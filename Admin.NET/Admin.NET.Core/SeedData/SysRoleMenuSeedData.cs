namespace Admin.NET.Core;

/// <summary>
/// 系统角色菜单表种子数据
/// </summary>
public class SysRoleMenuSeedData : ISqlSugarEntitySeedData<SysRoleMenu>
{
    /// <summary>
    /// 种子数据
    /// </summary>
    /// <returns></returns>
    [IgnoreUpdate]
    public IEnumerable<SysRoleMenu> HasData()
    {
        return new[]
        {
            // 数据面板【admin/252885263003721】
            new SysRoleMenu{ Id=252885263003000, RoleId=252885263003721, MenuId=252885263002100 },
            new SysRoleMenu{ Id=252885263003001, RoleId=252885263003721, MenuId=252885263002110 },
            new SysRoleMenu{ Id=252885263003002, RoleId=252885263003721, MenuId=252885263002111 },

            // 系统管理
            new SysRoleMenu{ Id=252885263003100, RoleId=252885263003721, MenuId=252885263005200 },
            // 账号管理
            new SysRoleMenu{ Id=252885263003101, RoleId=252885263003721, MenuId=252885263005210 },
            new SysRoleMenu{ Id=252885263003102, RoleId=252885263003721, MenuId=252885263005211 },
            new SysRoleMenu{ Id=252885263003103, RoleId=252885263003721, MenuId=252885263005212 },
            new SysRoleMenu{ Id=252885263003104, RoleId=252885263003721, MenuId=252885263005213 },
            new SysRoleMenu{ Id=252885263003105, RoleId=252885263003721, MenuId=252885263005214 },
            new SysRoleMenu{ Id=252885263003106, RoleId=252885263003721, MenuId=252885263005215 },
            new SysRoleMenu{ Id=252885263003107, RoleId=252885263003721, MenuId=252885263005216 },
            new SysRoleMenu{ Id=252885263003108, RoleId=252885263003721, MenuId=252885263005217 },
            new SysRoleMenu{ Id=252885263003109, RoleId=252885263003721, MenuId=252885263005218 },
            new SysRoleMenu{ Id=252885263003110, RoleId=252885263003721, MenuId=252885263005219 },
            new SysRoleMenu{ Id=252885263013111, RoleId=252885263003721, MenuId=252885263005220 },
            // 角色管理
            new SysRoleMenu{ Id=252885263003111, RoleId=252885263003721, MenuId=252885263005230 },
            new SysRoleMenu{ Id=252885263003112, RoleId=252885263003721, MenuId=252885263005231 },
            new SysRoleMenu{ Id=252885263003113, RoleId=252885263003721, MenuId=252885263005232 },
            new SysRoleMenu{ Id=252885263003114, RoleId=252885263003721, MenuId=252885263005233 },
            new SysRoleMenu{ Id=252885263003115, RoleId=252885263003721, MenuId=252885263005234 },
            new SysRoleMenu{ Id=252885263003116, RoleId=252885263003721, MenuId=252885263005235 },
            new SysRoleMenu{ Id=252885263003117, RoleId=252885263003721, MenuId=252885263005236 },
            new SysRoleMenu{ Id=252885263003118, RoleId=252885263003721, MenuId=252885263005237 },
            // 机构管理
            new SysRoleMenu{ Id=252885263003131, RoleId=252885263003721, MenuId=252885263005240 },
            new SysRoleMenu{ Id=252885263003132, RoleId=252885263003721, MenuId=252885263005241 },
            new SysRoleMenu{ Id=252885263003133, RoleId=252885263003721, MenuId=252885263005242 },
            new SysRoleMenu{ Id=252885263003134, RoleId=252885263003721, MenuId=252885263005243 },
            new SysRoleMenu{ Id=252885263003135, RoleId=252885263003721, MenuId=252885263005244 },
            // 职位管理
            new SysRoleMenu{ Id=252885263003141, RoleId=252885263003721, MenuId=252885263005250 },
            new SysRoleMenu{ Id=252885263003142, RoleId=252885263003721, MenuId=252885263005251 },
            new SysRoleMenu{ Id=252885263003143, RoleId=252885263003721, MenuId=252885263005252 },
            new SysRoleMenu{ Id=252885263003144, RoleId=252885263003721, MenuId=252885263005253 },
            new SysRoleMenu{ Id=252885263003145, RoleId=252885263003721, MenuId=252885263005254 },
            // 个人中心
            new SysRoleMenu{ Id=252885263003151, RoleId=252885263003721, MenuId=252885263005260 },
            new SysRoleMenu{ Id=252885263003152, RoleId=252885263003721, MenuId=252885263005261 },
            new SysRoleMenu{ Id=252885263003153, RoleId=252885263003721, MenuId=252885263005262 },
            new SysRoleMenu{ Id=252885263003154, RoleId=252885263003721, MenuId=252885263005263 },
            // 通知公告
            new SysRoleMenu{ Id=252885263003161, RoleId=252885263003721, MenuId=252885263005270 },
            new SysRoleMenu{ Id=252885263003162, RoleId=252885263003721, MenuId=252885263005271 },
            new SysRoleMenu{ Id=252885263003163, RoleId=252885263003721, MenuId=252885263005272 },
            new SysRoleMenu{ Id=252885263003164, RoleId=252885263003721, MenuId=252885263005273 },
            new SysRoleMenu{ Id=252885263003165, RoleId=252885263003721, MenuId=252885263005274 },
            new SysRoleMenu{ Id=252885263003166, RoleId=252885263003721, MenuId=252885263005275 },
            new SysRoleMenu{ Id=252885263003167, RoleId=252885263003721, MenuId=252885263005276 },

            // 平台管理
            new SysRoleMenu{ Id=252885263003200, RoleId=252885263003721, MenuId=252885263005300 },
            //// 参数配置
            //new SysRoleMenu{ Id=252885263003201, RoleId=252885263003721, MenuId=252885263005320 },
            //new SysRoleMenu{ Id=252885263003202, RoleId=252885263003721, MenuId=252885263005321 },
            //new SysRoleMenu{ Id=252885263003203, RoleId=252885263003721, MenuId=252885263005322 },
            //new SysRoleMenu{ Id=252885263003204, RoleId=252885263003721, MenuId=252885263005323 },
            //new SysRoleMenu{ Id=252885263003205, RoleId=252885263003721, MenuId=252885263005324 },
            //// 字典管理
            //new SysRoleMenu{ Id=252885263003211, RoleId=252885263003721, MenuId=252885263005330 },
            //new SysRoleMenu{ Id=252885263003212, RoleId=252885263003721, MenuId=252885263005331 },
            //new SysRoleMenu{ Id=252885263003213, RoleId=252885263003721, MenuId=252885263005332 },
            //new SysRoleMenu{ Id=252885263003214, RoleId=252885263003721, MenuId=252885263005333 },
            //new SysRoleMenu{ Id=252885263003215, RoleId=252885263003721, MenuId=252885263005334 },
            // 系统监控
            new SysRoleMenu{ Id=252885263003231, RoleId=252885263003721, MenuId=252885263005360 },
            // 缓存管理
            new SysRoleMenu{ Id=252885263003241, RoleId=252885263003721, MenuId=252885263005370 },
            new SysRoleMenu{ Id=252885263003242, RoleId=252885263003721, MenuId=252885263005371 },
            new SysRoleMenu{ Id=252885263003243, RoleId=252885263003721, MenuId=252885263005372 },
            // 行政区域
            new SysRoleMenu{ Id=252885263003251, RoleId=252885263003721, MenuId=252885263005380 },
            new SysRoleMenu{ Id=252885263003252, RoleId=252885263003721, MenuId=252885263005381 },
            new SysRoleMenu{ Id=252885263003253, RoleId=252885263003721, MenuId=252885263005382 },
            new SysRoleMenu{ Id=252885263003254, RoleId=252885263003721, MenuId=252885263005383 },
            new SysRoleMenu{ Id=252885263003255, RoleId=252885263003721, MenuId=252885263005384 },
            new SysRoleMenu{ Id=252885263003256, RoleId=252885263003721, MenuId=252885263005385 },
            // 文件管理
            new SysRoleMenu{ Id=252885263003261, RoleId=252885263003721, MenuId=252885263005390 },
            new SysRoleMenu{ Id=252885263003262, RoleId=252885263003721, MenuId=252885263005391 },
            new SysRoleMenu{ Id=252885263003263, RoleId=252885263003721, MenuId=252885263005392 },
            new SysRoleMenu{ Id=252885263003264, RoleId=252885263003721, MenuId=252885263005393 },
            new SysRoleMenu{ Id=252885263003265, RoleId=252885263003721, MenuId=252885263005394 },

            // 日志管理
            new SysRoleMenu{ Id=252885263003300, RoleId=252885263003721, MenuId=252885263005500 },
            new SysRoleMenu{ Id=252885263003301, RoleId=252885263003721, MenuId=252885263005510 },
            new SysRoleMenu{ Id=252885263003302, RoleId=252885263003721, MenuId=252885263005511 },
            new SysRoleMenu{ Id=252885263003311, RoleId=252885263003721, MenuId=252885263005520 },
            new SysRoleMenu{ Id=252885263003312, RoleId=252885263003721, MenuId=252885263005521 },
            new SysRoleMenu{ Id=252885263003321, RoleId=252885263003721, MenuId=252885263005530 },
            new SysRoleMenu{ Id=252885263003322, RoleId=252885263003721, MenuId=252885263005531 },
            new SysRoleMenu{ Id=252885263003331, RoleId=252885263003721, MenuId=252885263005540 },
            new SysRoleMenu{ Id=252885263003332, RoleId=252885263003721, MenuId=252885263005541 },

            // 帮助文档
            new SysRoleMenu{ Id=252885263003500, RoleId=252885263003721, MenuId=252885263005700 },
            new SysRoleMenu{ Id=252885263003501, RoleId=252885263003721, MenuId=252885263005710 },
            new SysRoleMenu{ Id=252885263003502, RoleId=252885263003721, MenuId=252885263005711 },

            // 数据面板【user1/252885263003722】
            new SysRoleMenu{ Id=252885263004000, RoleId=252885263003722, MenuId=252885263002100 },
            new SysRoleMenu{ Id=252885263004001, RoleId=252885263003722, MenuId=252885263002110 },
            new SysRoleMenu{ Id=252885263004002, RoleId=252885263003722, MenuId=252885263002111 },
            // 系统管理
            new SysRoleMenu{ Id=252885263004100, RoleId=252885263003722, MenuId=252885263005200 },
            // 个人中心
            new SysRoleMenu{ Id=252885263004151, RoleId=252885263003722, MenuId=252885263005260 },
            new SysRoleMenu{ Id=252885263004152, RoleId=252885263003722, MenuId=252885263005261 },
            new SysRoleMenu{ Id=252885263004153, RoleId=252885263003722, MenuId=252885263005262 },
            new SysRoleMenu{ Id=252885263004154, RoleId=252885263003722, MenuId=252885263005263 },

            // 数据面板【user3/252885263003724】
            new SysRoleMenu{ Id=252885263005000, RoleId=252885263003724, MenuId=252885263002100 },
            new SysRoleMenu{ Id=252885263005001, RoleId=252885263003724, MenuId=252885263002110 },
            new SysRoleMenu{ Id=252885263005002, RoleId=252885263003724, MenuId=252885263002111 },
            // 系统管理
            new SysRoleMenu{ Id=252885263005100, RoleId=252885263003724, MenuId=252885263005200 },
            // 个人中心
            new SysRoleMenu{ Id=252885263005151, RoleId=252885263003724, MenuId=252885263005260},
            new SysRoleMenu{ Id=252885263005152, RoleId=252885263003724, MenuId=252885263005261 },
            new SysRoleMenu{ Id=252885263005153, RoleId=252885263003724, MenuId=252885263005262 },
            new SysRoleMenu{ Id=252885263005154, RoleId=252885263003724, MenuId=252885263005263 },
        };
    }
}