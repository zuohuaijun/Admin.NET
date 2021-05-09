using System;

namespace Admin.NET.Core.Service
{
    /// <summary>
    /// 用户参数
    /// </summary>
    public class UserOutput
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual string Id { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public virtual string Account { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public virtual string NickName { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public virtual string Avatar { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public virtual DateTimeOffset Birthday { get; set; }

        /// <summary>
        /// 性别-男_1、女_2
        /// </summary>
        public virtual int Sex { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public virtual string Phone { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public virtual string Tel { get; set; }

        /// <summary>
        /// 状态-正常_0、停用_1、删除_2
        /// </summary>
        public virtual int Status { get; set; }

        /// <summary>
        /// 员工信息
        /// </summary>
        public EmpOutput SysEmpInfo { get; set; }
    }
}