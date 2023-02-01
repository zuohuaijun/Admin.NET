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
            // 数据面板【admin/1300000000101】
            new SysRoleMenu{ Id=1300000000101, RoleId=1300000000101, MenuId=1300000000101 },
            new SysRoleMenu{ Id=1300000000102, RoleId=1300000000101, MenuId=1300000000111 },
            new SysRoleMenu{ Id=1300000000103, RoleId=1300000000101, MenuId=1300000000121 },

            // 系统管理
            new SysRoleMenu{ Id=1300000000111, RoleId=1300000000101, MenuId=1310000000101 },
            // 账号管理
            new SysRoleMenu{ Id=1300000000121, RoleId=1300000000101, MenuId=1310000000111 },
            new SysRoleMenu{ Id=1300000000122, RoleId=1300000000101, MenuId=1310000000112 },
            new SysRoleMenu{ Id=1300000000123, RoleId=1300000000101, MenuId=1310000000113 },
            new SysRoleMenu{ Id=1300000000124, RoleId=1300000000101, MenuId=1310000000114 },
            new SysRoleMenu{ Id=1300000000125, RoleId=1300000000101, MenuId=1310000000115 },
            new SysRoleMenu{ Id=1300000000126, RoleId=1300000000101, MenuId=1310000000116 },
            new SysRoleMenu{ Id=1300000000127, RoleId=1300000000101, MenuId=1310000000117 },
            new SysRoleMenu{ Id=1300000000128, RoleId=1300000000101, MenuId=1310000000118 },
            new SysRoleMenu{ Id=1300000000129, RoleId=1300000000101, MenuId=1310000000119 },
            new SysRoleMenu{ Id=1300000000130, RoleId=1300000000101, MenuId=1310000000120 },
            new SysRoleMenu{ Id=1300000000131, RoleId=1300000000101, MenuId=1310000000121 },
            // 角色管理
            new SysRoleMenu{ Id=1300000000141, RoleId=1300000000101, MenuId=1310000000131 },
            new SysRoleMenu{ Id=1300000000142, RoleId=1300000000101, MenuId=1310000000132 },
            new SysRoleMenu{ Id=1300000000143, RoleId=1300000000101, MenuId=1310000000133 },
            new SysRoleMenu{ Id=1300000000144, RoleId=1300000000101, MenuId=1310000000134 },
            new SysRoleMenu{ Id=1300000000145, RoleId=1300000000101, MenuId=1310000000135 },
            new SysRoleMenu{ Id=1300000000146, RoleId=1300000000101, MenuId=1310000000136 },
            new SysRoleMenu{ Id=1300000000147, RoleId=1300000000101, MenuId=1310000000137 },
            new SysRoleMenu{ Id=1300000000148, RoleId=1300000000101, MenuId=1310000000138 },
            // 机构管理
            new SysRoleMenu{ Id=1300000000151, RoleId=1300000000101, MenuId=1310000000141 },
            new SysRoleMenu{ Id=1300000000152, RoleId=1300000000101, MenuId=1310000000142 },
            new SysRoleMenu{ Id=1300000000153, RoleId=1300000000101, MenuId=1310000000143 },
            new SysRoleMenu{ Id=1300000000154, RoleId=1300000000101, MenuId=1310000000144 },
            new SysRoleMenu{ Id=1300000000155, RoleId=1300000000101, MenuId=1310000000145 },
            // 职位管理
            new SysRoleMenu{ Id=1300000000161, RoleId=1300000000101, MenuId=1310000000151 },
            new SysRoleMenu{ Id=1300000000162, RoleId=1300000000101, MenuId=1310000000152 },
            new SysRoleMenu{ Id=1300000000163, RoleId=1300000000101, MenuId=1310000000153 },
            new SysRoleMenu{ Id=1300000000164, RoleId=1300000000101, MenuId=1310000000154 },
            new SysRoleMenu{ Id=1300000000165, RoleId=1300000000101, MenuId=1310000000155 },
            // 个人中心
            new SysRoleMenu{ Id=1300000000171, RoleId=1300000000101, MenuId=1310000000161 },
            new SysRoleMenu{ Id=1300000000172, RoleId=1300000000101, MenuId=1310000000162 },
            new SysRoleMenu{ Id=1300000000173, RoleId=1300000000101, MenuId=1310000000163 },
            new SysRoleMenu{ Id=1300000000174, RoleId=1300000000101, MenuId=1310000000164 },
            // 通知公告
            new SysRoleMenu{ Id=1300000000181, RoleId=1300000000101, MenuId=1310000000171 },
            new SysRoleMenu{ Id=1300000000182, RoleId=1300000000101, MenuId=1310000000172 },
            new SysRoleMenu{ Id=1300000000183, RoleId=1300000000101, MenuId=1310000000173 },
            new SysRoleMenu{ Id=1300000000184, RoleId=1300000000101, MenuId=1310000000174 },
            new SysRoleMenu{ Id=1300000000185, RoleId=1300000000101, MenuId=1310000000175 },
            new SysRoleMenu{ Id=1300000000186, RoleId=1300000000101, MenuId=1310000000176 },
            new SysRoleMenu{ Id=1300000000187, RoleId=1300000000101, MenuId=1310000000177 },
            // 三方账号
            new SysRoleMenu{ Id=1300000000191, RoleId=1300000000101, MenuId=1310000000181 },
            new SysRoleMenu{ Id=1300000000192, RoleId=1300000000101, MenuId=1310000000182 },
            new SysRoleMenu{ Id=1300000000193, RoleId=1300000000101, MenuId=1310000000183 },
            new SysRoleMenu{ Id=1300000000194, RoleId=1300000000101, MenuId=1310000000184 },

            //// 平台管理
            //new SysRoleMenu{ Id=1300000000201, RoleId=1300000000101, MenuId=1310000000301 },
            // 任务调度
            new SysRoleMenu{ Id=1300000000251, RoleId=1300000000101, MenuId=1310000000351 },
            new SysRoleMenu{ Id=1300000000252, RoleId=1300000000101, MenuId=1310000000352 },
            new SysRoleMenu{ Id=1300000000253, RoleId=1300000000101, MenuId=1310000000353 },
            new SysRoleMenu{ Id=1300000000254, RoleId=1300000000101, MenuId=1310000000354 },
            new SysRoleMenu{ Id=1300000000255, RoleId=1300000000101, MenuId=1310000000355 },
            // 系统监控
            new SysRoleMenu{ Id=1300000000261, RoleId=1300000000101, MenuId=1310000000361 },
            // 缓存管理
            new SysRoleMenu{ Id=1300000000271, RoleId=1300000000101, MenuId=1310000000371 },
            new SysRoleMenu{ Id=1300000000272, RoleId=1300000000101, MenuId=1310000000372 },
            new SysRoleMenu{ Id=1300000000273, RoleId=1300000000101, MenuId=1310000000373 },
            // 行政区域
            new SysRoleMenu{ Id=1300000000281, RoleId=1300000000101, MenuId=1310000000381 },
            new SysRoleMenu{ Id=1300000000282, RoleId=1300000000101, MenuId=1310000000382 },
            new SysRoleMenu{ Id=1300000000283, RoleId=1300000000101, MenuId=1310000000383 },
            new SysRoleMenu{ Id=1300000000284, RoleId=1300000000101, MenuId=1310000000384 },
            new SysRoleMenu{ Id=1300000000285, RoleId=1300000000101, MenuId=1310000000385 },
            new SysRoleMenu{ Id=1300000000286, RoleId=1300000000101, MenuId=1310000000386 },
            // 文件管理
            new SysRoleMenu{ Id=1300000000291, RoleId=1300000000101, MenuId=1310000000391 },
            new SysRoleMenu{ Id=1300000000292, RoleId=1300000000101, MenuId=1310000000392 },
            new SysRoleMenu{ Id=1300000000293, RoleId=1300000000101, MenuId=1310000000393 },
            new SysRoleMenu{ Id=1300000000294, RoleId=1300000000101, MenuId=1310000000394 },
            new SysRoleMenu{ Id=1300000000295, RoleId=1300000000101, MenuId=1310000000395 },

            //// 日志管理
            //new SysRoleMenu{ Id=1300000000301, RoleId=1300000000101, MenuId=1310000000501 },
            new SysRoleMenu{ Id=1300000000311, RoleId=1300000000101, MenuId=1310000000511 },
            new SysRoleMenu{ Id=1300000000312, RoleId=1300000000101, MenuId=1310000000512 },
            new SysRoleMenu{ Id=1300000000321, RoleId=1300000000101, MenuId=1310000000521 },
            new SysRoleMenu{ Id=1300000000322, RoleId=1300000000101, MenuId=1310000000522 },
            new SysRoleMenu{ Id=1300000000331, RoleId=1300000000101, MenuId=1310000000531 },
            new SysRoleMenu{ Id=1300000000332, RoleId=1300000000101, MenuId=1310000000532 },
            new SysRoleMenu{ Id=1300000000341, RoleId=1300000000101, MenuId=1310000000541 },
            new SysRoleMenu{ Id=1300000000342, RoleId=1300000000101, MenuId=1310000000542 },

            // 帮助文档
            new SysRoleMenu{ Id=1300000000401, RoleId=1300000000101, MenuId=1310000000701 },
            new SysRoleMenu{ Id=1300000000402, RoleId=1300000000101, MenuId=1310000000711 },
            new SysRoleMenu{ Id=1300000000403, RoleId=1300000000101, MenuId=1310000000712 },

            // 其他角色默认菜单
            // 数据面板【1300000000102】
            new SysRoleMenu{ Id=1300000000501, RoleId=1300000000102, MenuId=1300000000101 },
            new SysRoleMenu{ Id=1300000000502, RoleId=1300000000102, MenuId=1300000000111 },
            new SysRoleMenu{ Id=1300000000503, RoleId=1300000000102, MenuId=1300000000121 },
            // 机构管理
            new SysRoleMenu{ Id=1300000000511, RoleId=1300000000102, MenuId=1310000000142 },
            // 个人中心
            new SysRoleMenu{ Id=1300000000521, RoleId=1300000000102, MenuId=1310000000161 },
            new SysRoleMenu{ Id=1300000000522, RoleId=1300000000102, MenuId=1310000000162 },
            new SysRoleMenu{ Id=1300000000523, RoleId=1300000000102, MenuId=1310000000163 },
            new SysRoleMenu{ Id=1300000000524, RoleId=1300000000102, MenuId=1310000000164 },

            // 数据面板【1300000000103】
            new SysRoleMenu{ Id=1300000000601, RoleId=1300000000103, MenuId=1300000000101 },
            new SysRoleMenu{ Id=1300000000602, RoleId=1300000000103, MenuId=1300000000111 },
            new SysRoleMenu{ Id=1300000000603, RoleId=1300000000103, MenuId=1300000000121 },
            // 机构管理
            new SysRoleMenu{ Id=1300000000611, RoleId=1300000000103, MenuId=1310000000142 },
            // 个人中心
            new SysRoleMenu{ Id=1300000000621, RoleId=1300000000103, MenuId=1310000000161 },
            new SysRoleMenu{ Id=1300000000622, RoleId=1300000000103, MenuId=1310000000162 },
            new SysRoleMenu{ Id=1300000000623, RoleId=1300000000103, MenuId=1310000000163 },
            new SysRoleMenu{ Id=1300000000624, RoleId=1300000000103, MenuId=1310000000164 },

            // 数据面板【1300000000104】
            new SysRoleMenu{ Id=1300000000701, RoleId=1300000000104, MenuId=1300000000101 },
            new SysRoleMenu{ Id=1300000000702, RoleId=1300000000104, MenuId=1300000000111 },
            new SysRoleMenu{ Id=1300000000703, RoleId=1300000000104, MenuId=1300000000121 },
            // 机构管理
            new SysRoleMenu{ Id=1300000000711, RoleId=1300000000104, MenuId=1310000000142 },
            // 个人中心
            new SysRoleMenu{ Id=1300000000721, RoleId=1300000000104, MenuId=1310000000161 },
            new SysRoleMenu{ Id=1300000000722, RoleId=1300000000104, MenuId=1310000000162 },
            new SysRoleMenu{ Id=1300000000723, RoleId=1300000000104, MenuId=1310000000163 },
            new SysRoleMenu{ Id=1300000000724, RoleId=1300000000104, MenuId=1310000000164 },

            // 数据面板【1300000000105】
            new SysRoleMenu{ Id=1300000000801, RoleId=1300000000105, MenuId=1300000000101 },
            new SysRoleMenu{ Id=1300000000802, RoleId=1300000000105, MenuId=1300000000111 },
            new SysRoleMenu{ Id=1300000000803, RoleId=1300000000105, MenuId=1300000000121 },
            // 机构管理
            new SysRoleMenu{ Id=1300000000811, RoleId=1300000000105, MenuId=1310000000142 },
            // 个人中心
            new SysRoleMenu{ Id=1300000000821, RoleId=1300000000105, MenuId=1310000000161 },
            new SysRoleMenu{ Id=1300000000822, RoleId=1300000000105, MenuId=1310000000162 },
            new SysRoleMenu{ Id=1300000000823, RoleId=1300000000105, MenuId=1310000000163 },
            new SysRoleMenu{ Id=1300000000824, RoleId=1300000000105, MenuId=1310000000164 },
        };
    }
}