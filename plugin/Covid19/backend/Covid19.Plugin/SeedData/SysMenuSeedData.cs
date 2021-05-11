using Admin.NET.Core;
using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Covid19.Plugin
{
    /// <summary>
    /// 系统菜单表种子数据
    /// </summary>
    public class SysMenuSeedData : IEntitySeedData<SysMenu>
    {
        /// <summary>
        /// 种子数据
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        /// <returns></returns>
        public IEnumerable<SysMenu> HasData(DbContext dbContext, Type dbContextLocator)
        {
            return new[]
            {   
                new SysMenu{Id=142307070926921, Pid=0, Pids="[0],", Name="核酸检测", Code="xg_hsjc", Type=MenuType.DIR, Icon="database", Router="/xghs", Component="PageView", Application="busiapp", OpenType=0, Visible="Y", Weight=MenuWeight.DEFAULT_WEIGHT, Sort=2, Status=0 },
                new SysMenu{Id=142307070926922, Pid=142307070926921, Pids="[0],[142307070926921],", Name="类型定义", Code="xg_lxdy", Type=MenuType.MENU, Icon="profile", Router="/xgtestitem", Component="main/covid19/xgTestItem/index", Application="busiapp", OpenType=MenuOpenType.NONE, Visible="Y", Weight=MenuWeight.DEFAULT_WEIGHT, Sort=100, Status=0 },
                new SysMenu{Id=142307070926923, Pid=142307070926922, Pids="[0],[142307070926921],[142307070926922],", Name="新增", Code="xg_lxdy_add", Type=MenuType.BTN, Permission="xgTestItem:add", Application="busiapp", OpenType=0, Visible="Y", Weight=MenuWeight.DEFAULT_WEIGHT, Sort=100, Status=0 },
                new SysMenu{Id=142307070926924, Pid=142307070926922, Pids="[0],[142307070926921],[142307070926922],", Name="编辑", Code="xg_lxdy_edit", Type=MenuType.BTN, Permission="xgTestItem:edit", Application="busiapp", OpenType=0, Visible="Y", Weight=MenuWeight.DEFAULT_WEIGHT, Sort=100, Status=0 },
                new SysMenu{Id=142307070926925, Pid=142307070926922, Pids="[0],[142307070926921],[142307070926922],", Name="删除", Code="xg_lxdy_delete", Type=MenuType.BTN, Permission="xgTestItem:delete", Application="busiapp", OpenType=0, Visible="Y", Weight=MenuWeight.DEFAULT_WEIGHT, Sort=100, Status=0 },
                new SysMenu{Id=142307070926926, Pid=142307070926922, Pids="[0],[142307070926921],[142307070926922],", Name="查询", Code="xg_lxdy_list", Type=MenuType.BTN, Permission="xgTestItem:page", Application="busiapp", OpenType=0, Visible="Y", Weight=MenuWeight.DEFAULT_WEIGHT, Sort=100, Status=0 },
                new SysMenu{Id=142307070926927, Pid=142307070926921, Pids="[0],[142307070926921],", Name="样本采集", Code="xg_ybcj", Type=MenuType.MENU, Icon="contacts", Router="/xgCollector", Component="main/covid19/xgCollector/index", Application="busiapp", OpenType=MenuOpenType.NONE, Visible="Y", Weight=MenuWeight.DEFAULT_WEIGHT, Sort=100, Status=0 },
                new SysMenu{Id=142307070926928, Pid=142307070926927, Pids="[0],[142307070926921],[142307070926927],", Name="新增", Code="xg_ybcj_add", Type=MenuType.BTN, Permission="xgCollector:add", Application="busiapp", OpenType=0, Visible="Y", Weight=MenuWeight.DEFAULT_WEIGHT, Sort=100, Status=0 },
                new SysMenu{Id=142307070926929, Pid=142307070926927, Pids="[0],[142307070926921],[142307070926927],", Name="编辑", Code="xg_ybcj_edit", Type=MenuType.BTN, Permission="xgCollector:edit", Application="busiapp", OpenType=0, Visible="Y", Weight=MenuWeight.DEFAULT_WEIGHT, Sort=100, Status=0 },
                new SysMenu{Id=142307070926930, Pid=142307070926927, Pids="[0],[142307070926921],[142307070926927],", Name="删除", Code="xg_ybcj_delete", Type=MenuType.BTN, Permission="xgCollector:delete", Application="busiapp", OpenType=0, Visible="Y", Weight=MenuWeight.DEFAULT_WEIGHT, Sort=100, Status=0 },
                new SysMenu{Id=142307070926931, Pid=142307070926927, Pids="[0],[142307070926921],[142307070926927],", Name="查询", Code="xg_ybcj_list", Type=MenuType.BTN, Permission="xgCollector:page", Application="busiapp", OpenType=0, Visible="Y", Weight=MenuWeight.DEFAULT_WEIGHT, Sort=100, Status=0 },
                new SysMenu{Id=142307070926932, Pid=142307070926927, Pids="[0],[142307070926921],[142307070926927],", Name="打印条码", Code="xg_ybcj_print", Type=MenuType.BTN, Permission="xgCollector:print", Application="busiapp", OpenType=0, Visible="Y", Weight=MenuWeight.DEFAULT_WEIGHT, Sort=100, Status=0 },
                new SysMenu{Id=142307070926933, Pid=142307070926921, Pids="[0],[142307070926921],", Name="样本检测", Code="xg_jchs", Type=MenuType.MENU, Icon="experiment", Router="/xgtest", Component="main/covid19/xgTest/index", Application="busiapp", OpenType=MenuOpenType.NONE, Visible="Y", Weight=MenuWeight.DEFAULT_WEIGHT, Sort=100, Status=0 },
                new SysMenu{Id=142307070926934, Pid=142307070926933, Pids="[0],[142307070926921],[142307070926933],", Name="检测", Code="xg_jchs_edit", Type=MenuType.BTN, Permission="xgTest:updateTestResult", Application="busiapp", OpenType=0, Visible="Y", Weight=MenuWeight.DEFAULT_WEIGHT, Sort=100, Status=0 },
                new SysMenu{Id=142307070926935, Pid=142307070926933, Pids="[0],[142307070926921],[142307070926933],", Name="审核", Code="xg_jchs_check", Type=MenuType.BTN, Permission="xgTest:checkTestResult", Application="busiapp", OpenType=0, Visible="Y", Weight=MenuWeight.DEFAULT_WEIGHT, Sort=100, Status=0 },
                new SysMenu{Id=142307070926936, Pid=142307070926933, Pids="[0],[142307070926921],[142307070926933],", Name="批量设置", Code="xg_jchs_batch_edit", Type=MenuType.BTN, Permission="xgTest:updateNegative", Application="busiapp", OpenType=0, Visible="Y", Weight=MenuWeight.DEFAULT_WEIGHT, Sort=100, Status=0 },
                new SysMenu{Id=142307070926937, Pid=142307070926933, Pids="[0],[142307070926921],[142307070926933],", Name="打印报告", Code="xg_jchs_print", Type=MenuType.BTN, Permission="xgTest:print", Application="busiapp", OpenType=0, Visible="Y", Weight=MenuWeight.DEFAULT_WEIGHT, Sort=100, Status=0 },
                new SysMenu{Id=142307070926938, Pid=142307070926933, Pids="[0],[142307070926921],[142307070926933],", Name="查询", Code="xg_jchs_list", Type=MenuType.BTN, Permission="xgTest:page", Application="busiapp", OpenType=0, Visible="Y", Weight=MenuWeight.DEFAULT_WEIGHT, Sort=100, Status=0 }
            };
        }
    }
}
