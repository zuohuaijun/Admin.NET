using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dilon.Core
{
    /// <summary>
    /// Oauth登录用户表
    /// </summary>
    [Table("sys_oauth_user")]
    [Comment("Oauth登录用户表")]
    public class SysOauthUser : DEntityBase
    {
        /// <summary>
        /// 第三方平台的用户唯一Id
        /// </summary>
        [Comment("UUID")]
        public string Uuid { get; set; }

        /// <summary>
        /// 用户授权的token
        /// </summary>
        [Comment("Token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [Comment("昵称")]
        public string NickName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [Comment("头像")]
        public string Avatar { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [Comment("性别")]
        public string Gender { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [Comment("电话")]
        public string Phone { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Comment("邮箱")]
        public string Email { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        [Comment("位置")]
        public string Location { get; set; }

        /// <summary>
        /// 用户网址
        /// </summary>
        [Comment("用户网址")]
        public string Blog { get; set; }

        /// <summary>
        /// 所在公司
        /// </summary>
        [Comment("所在公司")]
        public string Company { get; set; }

        /// <summary>
        /// 用户来源
        /// </summary>
        [Comment("用户来源")]
        public string Source { get; set; }

        /// <summary>
        /// 用户备注（各平台中的用户个人介绍）
        /// </summary>
        [Comment("备注")]
        public string Remark { get; set; }
    }
}