using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Dilon.Core
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
                new SysMenu{Id=1, Pid=0, Pids="[0],", Name="主控面板", Code="system_index", Type=0, Icon="home", Router="/", Component="RouteView", Application="system", OpenType=0, Visible="Y", Redirect="/analysis", Weight=1, Sort=1, Status=0 },
                new SysMenu{Id=2, Pid=1, Pids="[0],[1],", Name="分析页", Code="system_index_dashboard", Type=1, Router="analysis", Component="system/dashboard/Analysis", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=3, Pid=1, Pids="[0],[1],", Name="工作台", Code="system_index_workplace", Type=1, Router="workplace", Component="system/dashboard/Workplace", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },

                new SysMenu{Id=4, Pid=0, Pids="[0],", Name="组织架构", Code="sys_mgr", Type=0, Icon="team", Router="/sys", Component="PageView", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=5, Pid=4, Pids="[0],[4],", Name="用户管理", Code="sys_user_mgr", Type=1, Router="/mgr_user", Component="system/user/index", Application="system", OpenType=1, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=6, Pid=5, Pids="[0],[4],[5],", Name="用户查询", Code="sys_user_mgr_page", Type=2, Permission="sysUser:page", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=7, Pid=5, Pids="[0],[4],[5],", Name="用户编辑", Code="sys_user_mgr_edit", Type=2, Permission="sysUser:edit", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=8, Pid=5, Pids="[0],[4],[5],", Name="用户增加", Code="sys_user_mgr_add", Type=2, Permission="sysUser:add", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0},
                new SysMenu{Id=9, Pid=5, Pids="[0],[4],[5],", Name="用户删除", Code="sys_user_mgr_delete", Type=2, Permission="sysUser:delete", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0},
                new SysMenu{Id=10, Pid=5, Pids="[0],[4],[5],", Name="用户详情", Code="sys_user_mgr_detail", Type=2, Permission="sysUser:detail", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=11, Pid=5, Pids="[0],[4],[5],", Name="用户导出", Code="sys_user_mgr_export", Type=2, Permission="sysUser:export", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=12, Pid=5, Pids="[0],[4],[5],", Name="用户选择器", Code="sys_user_mgr_selector", Type=2, Permission="sysUser:selector", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=13, Pid=5, Pids="[0],[4],[5],", Name="用户授权角色", Code="sys_user_mgr_grant_role", Type=2, Permission="sysUser:grantRole", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=14, Pid=5, Pids="[0],[4],[5],", Name="用户拥有角色", Code="sys_user_mgr_own_role", Type=2, Permission="sysUser:ownRole", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=15, Pid=5, Pids="[0],[4],[5],", Name="用户授权数据", Code="sys_user_mgr_grant_data", Type=2, Permission="sysUser:grantData", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=16, Pid=5, Pids="[0],[4],[5],", Name="用户拥有数据", Code="sys_user_mgr_own_data", Type=2, Permission="sysUser:ownData", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=17, Pid=5, Pids="[0],[4],[5],", Name="用户更新信息", Code="sys_user_mgr_update_info", Type=2, Permission="sysUser:updateInfo", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=18, Pid=5, Pids="[0],[4],[5],", Name="用户修改密码", Code="sys_user_mgr_update_pwd", Type=2, Permission="sysUser:updatePwd", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=19, Pid=5, Pids="[0],[4],[5],", Name="用户修改状态", Code="sys_user_mgr_change_status", Type=2, Permission="sysUser:changeStatus", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=20, Pid=5, Pids="[0],[4],[5],", Name="用户修改头像", Code="sys_user_mgr_update_avatar", Type=2, Permission="sysUser:updateAvatar", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0},
                new SysMenu{Id=21, Pid=5, Pids="[0],[4],[5],", Name="用户重置密码", Code="sys_user_mgr_reset_pwd", Type=2, Permission="sysUser:resetPwd", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=22, Pid=4, Pids="[0],[4],", Name="机构管理", Code="sys_org_mgr", Type=1, Router="/org", Component="system/org/index", Application="system", OpenType=1, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=23, Pid=22, Pids="[0],[4],[22],", Name="机构查询", Code="sys_org_mgr_page", Type=2, Permission="sysOrg:page", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=24, Pid=22, Pids="[0],[4],[22],", Name="机构列表", Code="sys_org_mgr_list", Type=2, Permission="sysOrg:list", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=25, Pid=22, Pids="[0],[4],[22],", Name="机构增加", Code="sys_org_mgr_add", Type=2, Permission="sysOrg:add", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=26, Pid=22, Pids="[0],[4],[22],", Name="机构编辑", Code="sys_org_mgr_edit", Type=2, Permission="sysOrg:edit", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=27, Pid=22, Pids="[0],[4],[22],", Name="机构删除", Code="sys_org_mgr_delete", Type=2, Permission="sysOrg:delete", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0},
                new SysMenu{Id=28, Pid=22, Pids="[0],[4],[22],", Name="机构详情", Code="sys_org_mgr_detail", Type=2, Permission="sysOrg:detail", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=29, Pid=22, Pids="[0],[4],[22],", Name="机构树", Code="sys_org_mgr_tree", Type=2, Permission="sysOrg:tree", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0},
                new SysMenu{Id=30, Pid=4, Pids="[0],[4],", Name="职位管理", Code="sys_pos_mgr", Type=1, Router="/pos", Component="system/pos/index", Application="system", OpenType=1, Visible="Y", Weight=1, Sort=100, Status=0},
                new SysMenu{Id=31, Pid=30, Pids="[0],[4],[30],", Name="职位查询", Code="sys_pos_mgr_page", Type=2, Permission="sysPos:page", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=32, Pid=30, Pids="[0],[4],[30],", Name="职位列表", Code="sys_pos_mgr_list", Type=2, Permission="sysPos:list", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=33, Pid=30, Pids="[0],[4],[30],", Name="职位增加", Code="sys_pos_mgr_add", Type=2, Permission="sysPos:add", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=34, Pid=30, Pids="[0],[4],[30],", Name="职位编辑", Code="sys_pos_mgr_edit", Type=2, Permission="sysPos:edit", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0},
                new SysMenu{Id=35, Pid=30, Pids="[0],[4],[30],", Name="职位删除", Code="sys_pos_mgr_delete", Type=2, Permission="sysPos:delete", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=36, Pid=30, Pids="[0],[4],[30],", Name="职位详情", Code="sys_pos_mgr_detail", Type=2, Permission="sysPos:detail", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },

                new SysMenu{Id=37, Pid=0, Pids="[0],", Name="权限管理", Code="auth_manager", Type=0, Icon="safety-certificate", Router="/auth", Component="PageView", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=38, Pid=37, Pids="[0],[37],", Name="应用管理", Code="sys_app_mgr", Type=1, Router="/app", Component="system/app/index", Application="system", OpenType=1, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=39, Pid=38, Pids="[0],[37],[38],", Name="应用查询", Code="sys_app_mgr_page", Type=2, Permission="sysApp:page", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=40, Pid=38, Pids="[0],[37],[38],", Name="应用列表", Code="sys_app_mgr_list", Type=2, Permission="sysApp:list", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=41, Pid=38, Pids="[0],[37],[38],", Name="应用增加", Code="sys_app_mgr_add", Type=2, Permission="sysApp:add", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=42, Pid=38, Pids="[0],[37],[38],", Name="应用编辑", Code="sys_app_mgr_edit", Type=2, Permission="sysApp:edit", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=43, Pid=38, Pids="[0],[37],[38],", Name="应用删除", Code="sys_app_mgr_delete", Type=2, Permission="sysApp:delete", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=44, Pid=38, Pids="[0],[37],[38],", Name="应用详情", Code="sys_app_mgr_detail", Type=2, Permission="sysApp:detail", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=45, Pid=38, Pids="[0],[37],[38],", Name="设为默认应用", Code="sys_app_mgr_set_as_default", Type=2, Permission="sysApp:setAsDefault", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=46, Pid=37, Pids="[0],[37],", Name="菜单管理", Code="sys_menu_mgr", Type=1, Router="/menu", Component="system/menu/index", Application="system", OpenType=1, Visible="Y", Weight=1, Sort=100, Status=0},
                new SysMenu{Id=47, Pid=46, Pids="[0],[37],[46],", Name="菜单列表", Code="sys_menu_mgr_list", Type=2, Permission="sysMenu:list", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=48, Pid=46, Pids="[0],[37],[46],", Name="菜单增加", Code="sys_menu_mgr_add", Type=2, Permission="sysMenu:add", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=49, Pid=46, Pids="[0],[37],[46],", Name="菜单编辑", Code="sys_menu_mgr_edit", Type=2, Permission="sysMenu:edit", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=50, Pid=46, Pids="[0],[37],[46],", Name="菜单删除", Code="sys_menu_mgr_delete", Type=2, Permission="sysMenu:delete", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=51, Pid=46, Pids="[0],[37],[46],", Name="菜单详情", Code="sys_menu_mgr_detail", Type=2, Permission="sysMenu:detail", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=52, Pid=46, Pids="[0],[37],[46],", Name="菜单授权树", Code="sys_menu_mgr_grant_tree", Type=2, Permission="sysMenu:treeForGrant", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=53, Pid=46, Pids="[0],[37],[46],", Name="菜单树", Code="sys_menu_mgr_tree", Type=2, Permission="sysMenu:tree", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=54, Pid=46, Pids="[0],[37],[46],", Name="菜单切换", Code="sys_menu_mgr_change", Type=2, Permission="sysMenu:change", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=55, Pid=37, Pids="[0],[37],", Name="角色管理", Code="sys_role_mgr", Type=1, Router="/role", Component="system/role/index", Application="system", OpenType=1, Visible="Y", Weight=1, Sort=100, Status=0},
                new SysMenu{Id=56, Pid=55, Pids="[0],[37],[55],", Name="角色查询", Code="sys_role_mgr_page", Type=2, Permission="sysRole:page", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=57, Pid=55, Pids="[0],[37],[55],", Name="角色增加", Code="sys_role_mgr_add", Type=2, Permission="sysRole:add", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=58, Pid=55, Pids="[0],[37],[55],", Name="角色编辑", Code="sys_role_mgr_edit", Type=2, Permission="sysRole:edit", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=59, Pid=55, Pids="[0],[37],[55],", Name="角色删除", Code="sys_role_mgr_delete", Type=2, Permission="sysRole:delete", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=60, Pid=55, Pids="[0],[37],[55],", Name="角色详情", Code="sys_role_mgr_detail", Type=2, Permission="sysRole:detail", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=61, Pid=55, Pids="[0],[37],[55],", Name="角色下拉", Code="sys_role_mgr_drop_down", Type=2, Permission="sysRole:dropDown", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=62, Pid=55, Pids="[0],[37],[55],", Name="角色授权菜单", Code="sys_role_mgr_grant_menu", Type=2, Permission="sysRole:grantMenu", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=63, Pid=55, Pids="[0],[37],[55],", Name="角色拥有菜单", Code="sys_role_mgr_own_menu", Type=2, Permission="sysRole:ownMenu", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=64, Pid=55, Pids="[0],[37],[55],", Name="角色授权数据", Code="sys_role_mgr_grant_data", Type=2, Permission="sysRole:grantData", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=65, Pid=55, Pids="[0],[37],[55],", Name="角色拥有数据", Code="sys_role_mgr_own_data", Type=2, Permission="sysRole:ownData", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },

                new SysMenu{Id=66, Pid=0, Pids="[0],", Name="开发管理", Code="system_tools", Type=0, Icon="euro", Router="/tools", Component="PageView", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=67, Pid=66, Pids="[0],[66],", Name="系统配置", Code="system_tools_config", Type=1, Router="/config", Component="system/config/index", Application="system", OpenType=1, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=68, Pid=67, Pids="[0],[66],[67],", Name="配置查询", Code="system_tools_config_page", Type=2, Permission="sysConfig:page", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=69, Pid=67, Pids="[0],[66],[67],", Name="配置列表", Code="system_tools_config_list", Type=2, Permission="sysConfig:list", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=70, Pid=67, Pids="[0],[66],[67],", Name="配置增加", Code="system_tools_config_add", Type=2, Permission="sysConfig:add", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=71, Pid=67, Pids="[0],[66],[67],", Name="配置编辑", Code="system_tools_config_edit", Type=2, Permission="sysConfig:edit", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=72, Pid=67, Pids="[0],[66],[67],", Name="配置删除", Code="system_tools_config_delete", Type=2, Permission="sysConfig:delete", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=73, Pid=67, Pids="[0],[66],[67],", Name="配置详情", Code="system_tools_config_detail", Type=2, Permission="sysConfig:detail", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=74, Pid=67, Pids="[0],[66],[67],", Name="设为默认应用", Code="sys_app_mgr_set_as_default", Type=2, Permission="sysApp:setAsDefault", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=75, Pid=66, Pids="[0],[66],", Name="邮件发送", Code="sys_email_mgr", Type=1, Router="/email", Component="system/email/index", Application="system", OpenType=1, Visible="Y", Weight=1, Sort=100, Status=0},
                new SysMenu{Id=76, Pid=75, Pids="[0],[66],[75],", Name="发送文本邮件", Code="sys_email_mgr_send_email", Type=2, Permission="email:sendEmail", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=77, Pid=75, Pids="[0],[66],[75],", Name="发送html邮件", Code="sys_email_mgr_send_email_html", Type=2, Permission="email:sendEmailHtml", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=78, Pid=66, Pids="[0],[66],", Name="短信管理", Code="sys_sms_mgr", Type=1, Router="/sms", Component="system/sms/index", Application="system", OpenType=1, Visible="Y", Weight=1, Sort=100, Status=0},
                new SysMenu{Id=79, Pid=78, Pids="[0],[66],[78],", Name="短信发送查询", Code="sys_sms_mgr_page", Type=2, Permission="sms:page", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=80, Pid=78, Pids="[0],[66],[78],", Name="发送验证码短信", Code="sys_sms_mgr_send_login_message", Type=2, Permission="sms:sendLoginMessage", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=81, Pid=78, Pids="[0],[66],[78],", Name="验证短信验证码", Code="sys_sms_mgr_validate_message", Type=2, Permission="sms:validateMessage", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=82, Pid=66, Pids="[0],[66],", Name="字典管理", Code="sys_dict_mgr", Type=1, Router="/dict", Component="system/dict/index", Application="system", OpenType=1, Visible="Y", Weight=1, Sort=100, Status=0},
                new SysMenu{Id=83, Pid=82, Pids="[0],[66],[82],", Name="字典类型查询", Code="sys_dict_mgr_dict_type_page", Type=2, Permission="sysDictType:page", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=84, Pid=82, Pids="[0],[66],[82],", Name="字典类型列表", Code="sys_dict_mgr_dict_type_list", Type=2, Permission="sysDictType:list", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=85, Pid=82, Pids="[0],[66],[82],", Name="字典类型增加", Code="sys_dict_mgr_dict_type_add", Type=2, Permission="sysDictType:add", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=86, Pid=82, Pids="[0],[66],[82],", Name="字典类型删除", Code="sys_dict_mgr_dict_type_delete", Type=2, Permission="sysDictType:delete", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=87, Pid=82, Pids="[0],[66],[82],", Name="字典类型编辑", Code="sys_dict_mgr_dict_type_edit", Type=2, Permission="sysDictType:edit", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=88, Pid=82, Pids="[0],[66],[82],", Name="字典类型详情", Code="sys_dict_mgr_dict_type_detail", Type=2, Permission="sysDictType:detail", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=89, Pid=82, Pids="[0],[66],[82],", Name="字典类型下拉", Code="sys_dict_mgr_dict_type_drop_down", Type=2, Permission="sysDictType:dropDown", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=90, Pid=82, Pids="[0],[66],[82],", Name="字典类型修改状态", Code="sys_dict_mgr_dict_type_change_status", Type=2, Permission="sysDictType:changeStatus", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=91, Pid=82, Pids="[0],[66],[82],", Name="字典值查询", Code="sys_dict_mgr_dict_page", Type=2, Permission="sysDictData:page", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=92, Pid=82, Pids="[0],[66],[82],", Name="字典值列表", Code="sys_dict_mgr_dict_list", Type=2, Permission="sysDictData:list", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=93, Pid=82, Pids="[0],[66],[82],", Name="字典值增加", Code="sys_dict_mgr_dict_add", Type=2, Permission="sysDictData:add", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=94, Pid=82, Pids="[0],[66],[82],", Name="字典值删除", Code="sys_dict_mgr_dict_delete", Type=2, Permission="sysDictData:delete", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=95, Pid=82, Pids="[0],[66],[82],", Name="字典值编辑", Code="sys_dict_mgr_dict_edit", Type=2, Permission="sysDictData:edit", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=96, Pid=82, Pids="[0],[66],[82],", Name="字典值详情", Code="sys_role_mgr_grant_data", Type=2, Permission="sysDictData:detail", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=97, Pid=82, Pids="[0],[66],[82],", Name="字典值修改状态", Code="sys_dict_mgr_dict_change_status", Type=2, Permission="sysDictData:changeStatus", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=98, Pid=66, Pids="[0],[66],", Name="接口文档", Code="sys_swagger_mgr", Type=1, Router="/swagger", Component="Iframe", Application="system", OpenType=2, Visible="Y", Link="http://127.0.0.1:5566/", Weight=1, Sort=100, Status=0},

                new SysMenu{Id=99, Pid=0, Pids="[0],", Name="日志管理", Code="sys_log_mgr", Type=0, Icon="read", Router="/log", Component="PageView", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=100, Pid=99, Pids="[0],[99],", Name="访问日志", Code="sys_log_mgr_vis_log", Type=1, Router="/vislog", Component="system/log/vislog/index", Application="system", OpenType=1, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=101, Pid=100, Pids="[0],[99],[100],", Name="访问日志查询", Code="sys_log_mgr_vis_log_page", Type=2, Permission="sysVisLog:page", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=102, Pid=100, Pids="[0],[99],[100],", Name="访问日志清空", Code="sys_log_mgr_vis_log_delete", Type=2, Permission="sysVisLog:delete", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=103, Pid=99, Pids="[0],[99],", Name="操作日志", Code="sys_log_mgr_op_log", Type=1, Router="/oplog", Component="system/log/oplog/index", Application="system", OpenType=1, Visible="Y", Weight=1, Sort=100, Status=0},
                new SysMenu{Id=104, Pid=103, Pids="[0],[99],[103],", Name="操作日志查询", Code="sys_log_mgr_op_log_page", Type=2, Permission="sysOpLog:page", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=105, Pid=103, Pids="[0],[99],[103],", Name="操作日志清空", Code="sys_log_mgr_op_log_delete", Type=2, Permission="sysOpLog:delete", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },

                new SysMenu{Id=106, Pid=0, Pids="[0],", Name="系统监控", Code="sys_monitor_mgr", Type=0, Icon="deployment-unit", Router="/monitor", Component="PageView", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=107, Pid=106, Pids="[0],[106],", Name="服务监控", Code="sys_monitor_mgr_machine_monitor", Type=1, Router="/machine", Component="system/machine/index", Application="system", OpenType=1, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=108, Pid=107, Pids="[0],[106],[107],", Name="服务监控查询", Code="sys_monitor_mgr_machine_monitor_query", Type=2, Permission="sysMachine:query", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=109, Pid=106, Pids="[0],[106],", Name="在线用户", Code="sys_monitor_mgr_online_user", Type=1, Router="/onlineUser", Component="system/onlineUser/index", Application="system", OpenType=1, Visible="Y", Weight=1, Sort=100, Status=0},
                new SysMenu{Id=110, Pid=109, Pids="[0],[106],[109],", Name="在线用户列表", Code="sys_monitor_mgr_online_user_list", Type=2, Permission="sysOnlineUser:list", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=111, Pid=109, Pids="[0],[106],[109],", Name="在线用户强退", Code="sys_monitor_mgr_online_user_force_exist", Type=2, Permission="sysOnlineUser:forceExist", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=112, Pid=106, Pids="[0],[106],", Name="数据监控", Code="sys_monitor_mgr_druid", Type=1, Router="/druid", Component="Iframe", Application="system", OpenType=2, Visible="N", Link="http://localhost:82/druid/login.html", Weight=1, Sort=100, Status=0},

                new SysMenu{Id=113, Pid=0, Pids="[0],", Name="通知公告", Code="sys_notice", Type=0, Icon="sound", Router="/notice", Component="PageView", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=114, Pid=113, Pids="[0],[113],", Name="公告管理", Code="sys_notice_mgr", Type=1, Router="/notice", Component="system/notice/index", Application="system", OpenType=1, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=115, Pid=114, Pids="[0],[113],[114],", Name="公告查询", Code="sys_notice_mgr_page", Type=2, Permission="sysNotice:page", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=116, Pid=114, Pids="[0],[113],[114],", Name="公告增加", Code="sys_notice_mgr_add", Type=2, Permission="sysNotice:add", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=117, Pid=114, Pids="[0],[113],[114],", Name="公告编辑", Code="sys_notice_mgr_edit", Type=2, Permission="sysNotice:edit", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=118, Pid=114, Pids="[0],[113],[114],", Name="公告删除", Code="sys_notice_mgr_delete", Type=2, Permission="sysNotice:delete", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=119, Pid=114, Pids="[0],[113],[114],", Name="公告查看", Code="sys_notice_mgr_detail", Type=2, Permission="sysNotice:detail", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=120, Pid=114, Pids="[0],[113],[114],", Name="公告修改状态", Code="sys_notice_mgr_changeStatus", Type=2, Permission="sysNotice:changeStatus", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=121, Pid=113, Pids="[0],[113],", Name="已收公告", Code="sys_notice_mgr_received", Type=1, Router="/noticeReceived", Component="system/noticeReceived/index", Application="system", OpenType=1, Visible="Y", Weight=1, Sort=100, Status=0},
                new SysMenu{Id=122, Pid=121, Pids="[0],[113],[121],", Name="已收公告查询", Code="sys_notice_mgr_received_page", Type=2, Permission="sysNotice:received", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },

                new SysMenu{Id=123, Pid=0, Pids="[0],", Name="文件管理", Code="sys_file_mgr", Type=0, Icon="file", Router="/file", Component="PageView", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=124, Pid=123, Pids="[0],[123],", Name="系统文件", Code="sys_file_mgr_sys_file", Type=1, Router="/file", Component="system/file/index", Application="system", OpenType=1, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=125, Pid=124, Pids="[0],[123],[124],", Name="文件查询", Code="sys_file_mgr_sys_file_page", Type=2, Permission="sysFileInfo:page", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=126, Pid=124, Pids="[0],[123],[124],", Name="文件列表", Code="sys_file_mgr_sys_file_list", Type=2, Permission="sysFileInfo:list", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=127, Pid=124, Pids="[0],[123],[124],", Name="文件删除", Code="sys_file_mgr_sys_file_delete", Type=2, Permission="sysFileInfo:delete", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=128, Pid=124, Pids="[0],[123],[124],", Name="文件详情", Code="sys_file_mgr_sys_file_detail", Type=2, Permission="sysFileInfo:detail", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=129, Pid=124, Pids="[0],[123],[124],", Name="文件上传", Code="sys_file_mgr_sys_file_upload", Type=2, Permission="sysFileInfo:upload", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=130, Pid=124, Pids="[0],[123],[124],", Name="文件下载", Code="sys_file_mgr_sys_file_download", Type=2, Permission="sysFileInfo:download", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=131, Pid=124, Pids="[0],[123],[124],", Name="图片预览", Code="sys_file_mgr_sys_file_preview", Type=2, Permission="sysFileInfo:preview", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },

                new SysMenu{Id=132, Pid=0, Pids="[0],", Name="定时任务", Code="sys_timers", Type=0, Icon="dashboard", Router="/timers", Component="PageView", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=133, Pid=132, Pids="[0],[132],", Name="任务管理", Code="sys_timers_mgr", Type=1, Router="/timers", Component="system/timers/index", Application="system", OpenType=1, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=134, Pid=133, Pids="[0],[132],[133],", Name="定时任务查询", Code="sys_timers_mgr_page", Type=2, Permission="sysTimers:page", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=135, Pid=133, Pids="[0],[132],[133],", Name="定时任务列表", Code="sys_timers_mgr_list", Type=2, Permission="sysTimers:list", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=136, Pid=133, Pids="[0],[132],[133],", Name="定时任务详情", Code="sys_timers_mgr_detail", Type=2, Permission="sysTimers:detail", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=137, Pid=133, Pids="[0],[132],[133],", Name="定时任务增加", Code="sys_timers_mgr_add", Type=2, Permission="sysTimers:add", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=138, Pid=133, Pids="[0],[132],[133],", Name="定时任务删除", Code="sys_timers_mgr_delete", Type=2, Permission="sysTimers:delete", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=139, Pid=133, Pids="[0],[132],[133],", Name="定时任务编辑", Code="sys_timers_mgr_edit", Type=2, Permission="sysTimers:edit", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=140, Pid=133, Pids="[0],[132],[133],", Name="定时任务可执行列表", Code="sys_timers_mgr_get_action_classes", Type=2, Permission="sysTimers:getActionClasses", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=141, Pid=133, Pids="[0],[132],[133],", Name="定时任务启动", Code="sys_timers_mgr_start", Type=2, Permission="sysTimers:start", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },
                new SysMenu{Id=142, Pid=133, Pids="[0],[132],[133],", Name="定时任务关闭", Code="sys_timers_mgr_stop", Type=2, Permission="sysTimers:stop", Application="system", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },

                new SysMenu{Id=143, Pid=0, Pids="[0],", Name="代码生成", Code="code_gen", Type=1, Icon="thunderbolt", Router="/codeGenerate/index", Component="gen/codeGenerate/index", Application="system_tool", OpenType=0, Visible="Y", Weight=1, Sort=100, Status=0 },

            };
        }
    }
}
