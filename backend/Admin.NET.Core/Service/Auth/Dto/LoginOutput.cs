using Furion.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Admin.NET.Core.Service
{
    /// <summary>
    /// 用户登录输出参数
    /// </summary>
    public class LoginOutput
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTimeOffset Birthday { get; set; }

        /// <summary>
        /// 性别(字典 1男 2女)
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public String Email { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public String Phone { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public String Tel { get; set; }

        /// <summary>
        /// 管理员类型（0超级管理员 1非管理员）
        /// </summary>
        public int AdminType { get; set; }

        /// <summary>
        /// 最后登陆IP
        /// </summary>
        public string LastLoginIp { get; set; }

        /// <summary>
        /// 最后登陆时间
        /// </summary>
        public DateTimeOffset LastLoginTime { get; set; }

        /// <summary>
        /// 最后登陆地址
        /// </summary>
        public string LastLoginAddress { get; set; }

        /// <summary>
        /// 最后登陆所用浏览器
        /// </summary>
        public string LastLoginBrowser { get; set; }

        /// <summary>
        /// 最后登陆所用系统
        /// </summary>
        public string LastLoginOs { get; set; }

        /// <summary>
        /// 员工信息
        /// </summary>
        public EmpOutput LoginEmpInfo { get; set; } = new EmpOutput();

        /// <summary>
        /// 具备应用信息
        /// </summary>
        public List<AppOutput> Apps { get; set; } = new List<AppOutput>();

        /// <summary>
        /// 角色信息
        /// </summary>
        public List<RoleOutput> Roles { get; set; } = new List<RoleOutput>();

        /// <summary>
        /// 权限信息
        /// </summary>
        public List<string> Permissions { get; set; } = new List<string>();

        /// <summary>
        /// 登录菜单信息---AntDesign版本菜单
        /// </summary>
        public List<AntDesignTreeNode> Menus { get; set; } = new List<AntDesignTreeNode>();

        /// <summary>
        /// 数据范围（机构）信息
        /// </summary>
        public List<long> DataScopes { get; set; } = new List<long>();

        ///// <summary>
        ///// 租户信息
        ///// </summary>
        //public List<long> Tenants { get; set; }

        ///// <summary>
        ///// 密码
        ///// </summary>
        //public string Password { get; set; }

        ///// <summary>
        ///// 账户过期
        ///// </summary>
        //public string AccountNonExpired { get; set; }

        ///// <summary>
        ///// 凭证过期
        ///// </summary>
        //public string CredentialsNonExpired { get; set; }

        ///// <summary>
        ///// 账户锁定
        ///// </summary>
        //public bool AccountNonLocked { get; set; }

        ///// <summary>
        ///// 用户名称
        ///// </summary>
        //public string UserName { get; set; }

        ///// <summary>
        ///// 权限
        ///// </summary>
        //public List<long> Authorities { get; set; } = new List<long>();

        ///// <summary>
        ///// 是否启动
        ///// </summary>
        //public bool Enabled { get; set; }
    }
}