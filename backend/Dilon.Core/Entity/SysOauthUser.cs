using Furion.Snowflake;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dilon.Core
{
    /// <summary>
    /// Oauth登录用户表
    /// </summary>
    [Table("sys_oauth_user")]
    public class SysOauthUser : DEntityBase
    {
        /// <summary>
        /// 第三方平台的用户唯一id
        /// </summary>
        public string Uuid { get; set; }

        /// <summary>
        /// 用户授权的token
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 用户网址
        /// </summary>
        public string Blog { get; set; }

        /// <summary>
        /// 所在公司
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// 用户来源
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 用户备注（各平台中的用户个人介绍）
        /// </summary>
        public string Remark { get; set; }
    }
}