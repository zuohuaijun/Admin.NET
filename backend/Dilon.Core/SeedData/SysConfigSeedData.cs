using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Dilon.Core
{
    /// <summary>
    /// 系统参数配置表种子数据
    /// </summary>
    public class SysConfigSeedData : IEntitySeedData<SysConfig>
    {
        /// <summary>
        /// 种子数据
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        /// <returns></returns>
        public IEnumerable<SysConfig> HasData(DbContext dbContext, Type dbContextLocator)
        {
            return new[]
            {
                new SysConfig{Id=1, Name="jwt密钥", Code="DILON_JWT_SECRET", Value="xiaonuo",SysFlag="Y", Remark="（重要）jwt密钥，默认为空，自行设置", Status=0, GroupCode="DEFAULT"},
                new SysConfig{Id=2, Name="默认密码", Code="DILON_DEFAULT_PASSWORD", Value="123456",SysFlag="Y", Remark="默认密码", Status=0, GroupCode="DEFAULT"},
                new SysConfig{Id=3, Name="token过期时间", Code="DILON_TOKEN_EXPIRE", Value="86400",SysFlag="Y", Remark="token过期时间（单位：秒）", Status=0, GroupCode="DEFAULT"},
                new SysConfig{Id=4, Name="session会话过期时间", Code="DILON_SESSION_EXPIRE", Value="7200",SysFlag="Y", Remark="session会话过期时间（单位：秒）", Status=0, GroupCode="DEFAULT"},
                new SysConfig{Id=5, Name="阿里云短信keyId", Code="DILON_ALIYUN_SMS_ACCESSKEY_ID", Value="你的keyId",SysFlag="Y", Remark="阿里云短信keyId", Status=0, GroupCode="ALIYUN_SMS"},
                new SysConfig{Id=6, Name="阿里云短信secret", Code="DILON_ALIYUN_SMS_ACCESSKEY_SECRET", Value="你的secret",SysFlag="Y", Remark="阿里云短信secret", Status=0, GroupCode="ALIYUN_SMS"},
                new SysConfig{Id=7, Name="阿里云短信签名", Code="DILON_ALIYUN_SMS_SIGN_NAME", Value="你的签名",SysFlag="Y", Remark="阿里云短信签名", Status=0, GroupCode="ALIYUN_SMS"},
                new SysConfig{Id=8, Name="阿里云短信-登录模板号", Code="DILON_ALIYUN_SMS_LOGIN_TEMPLATE_CODE", Value="SMS_1877123456",SysFlag="Y", Remark="阿里云短信-登录模板号", Status=0, GroupCode="ALIYUN_SMS"},
                new SysConfig{Id=9, Name="阿里云短信默认失效时间", Code="DILON_ALIYUN_SMS_INVALIDATE_MINUTES", Value="5",SysFlag="Y", Remark="阿里云短信默认失效时间（单位：分钟）", Status=0, GroupCode="ALIYUN_SMS"},
                new SysConfig{Id=10, Name="腾讯云短信secretId", Code="DILON_TENCENT_SMS_SECRET_ID", Value="你的secretId",SysFlag="Y", Remark="腾讯云短信secretId", Status=0, GroupCode="TENCENT_SMS"},
                new SysConfig{Id=11, Name="腾讯云短信secretKey", Code="DILON_TENCENT_SMS_SECRET_KEY", Value="你的secretkey",SysFlag="Y", Remark="腾讯云短信secretKey", Status=0, GroupCode="TENCENT_SMS"},
                new SysConfig{Id=12, Name="腾讯云短信sdkAppId", Code="DILON_TENCENT_SMS_SDK_APP_ID", Value="1400375123",SysFlag="Y", Remark="腾讯云短信sdkAppId", Status=0, GroupCode="TENCENT_SMS"},
                new SysConfig{Id=13, Name="腾讯云短信签名", Code="DILON_TENCENT_SMS_SIGN", Value="你的签名",SysFlag="Y", Remark="腾讯云短信签名", Status=0, GroupCode="TENCENT_SMS"},
                new SysConfig{Id=14, Name="邮箱host", Code="DILON_EMAIL_HOST", Value="smtp.126.com",SysFlag="Y", Remark="邮箱host", Status=0, GroupCode="EMAIL"},
                new SysConfig{Id=15, Name="邮箱用户名", Code="DILON_EMAIL_USERNAME", Value="test@126.com",SysFlag="Y", Remark="邮箱用户名", Status=0, GroupCode="EMAIL"},
                new SysConfig{Id=16, Name="邮箱密码", Code="DILON_EMAIL_PASSWORD", Value="你的邮箱密码",SysFlag="Y", Remark="邮箱密码", Status=0, GroupCode="EMAIL"},
                new SysConfig{Id=17, Name="邮箱端口", Code="DILON_EMAIL_PORT", Value="465",SysFlag="Y", Remark="邮箱端口", Status=0, GroupCode="EMAIL"},
                new SysConfig{Id=18, Name="邮箱是否开启ssl", Code="DILON_EMAIL_SSL", Value="true",SysFlag="Y", Remark="邮箱是否开启ssl", Status=0, GroupCode="EMAIL"},
                new SysConfig{Id=19, Name="邮箱发件人", Code="DILON_EMAIL_FROM", Value="test@126.com",SysFlag="Y", Remark="邮箱发件人", Status=0, GroupCode="EMAIL"},
                new SysConfig{Id=20, Name="Win本地上传文件路径", Code="DILON_FILE_UPLOAD_PATH_FOR_WINDOWS", Value="D:/tmp",SysFlag="Y", Remark="Win本地上传文件路径", Status=0, GroupCode="FILE_PATH"},
                new SysConfig{Id=21, Name="Linux/Mac本地上传文件路径", Code="DILON_FILE_UPLOAD_PATH_FOR_LINUX", Value="/tmp",SysFlag="Y", Remark="Linux/Mac本地上传文件路径", Status=0, GroupCode="FILE_PATH"},
                new SysConfig{Id=22, Name="放开XSS过滤的接口", Code="DILON_UN_XSS_FILTER_URL", Value="/demo/xssfilter,/demo/unxss",SysFlag="Y", Remark="多个url可以用英文逗号隔开", Status=0, GroupCode="DEFAULT"},
                new SysConfig{Id=23, Name="单用户登陆的开关", Code="DILON_ENABLE_SINGLE_LOGIN", Value="false",SysFlag="Y", Remark="true-打开，false-关闭，如果一个人登录两次，就会将上一次登陆挤下去", Status=0, GroupCode="DEFAULT"},
                new SysConfig{Id=24, Name="登录验证码的开关", Code="DILON_CAPTCHA_OPEN", Value="true",SysFlag="Y", Remark="true-打开，false-关闭", Status=0, GroupCode="DEFAULT"},
                new SysConfig{Id=25, Name="Druid监控登录账号", Code="DILON_DRUID_USERNAME", Value="superAdmin",SysFlag="Y", Remark="Druid监控登录账号", Status=0, GroupCode="DEFAULT"},
                new SysConfig{Id=26, Name="Druid监控界面登录密码", Code="DILON_DRUID_PASSWORD", Value="123456",SysFlag="Y", Remark="Druid监控界面登录密码", Status=0, GroupCode="DEFAULT"},
                new SysConfig{Id=27, Name="阿里云定位api接口地址", Code="DILON_IP_GEO_API", Value="http://api01.aliyun.venuscn.com/ip?ip=%s",SysFlag="Y", Remark="阿里云定位api接口地址", Status=0, GroupCode="DEFAULT"},
                new SysConfig{Id=28, Name="阿里云定位appCode", Code="DILON_IP_GEO_APP_CODE", Value="461535aabeae4f34861884d392f5d452",SysFlag="Y", Remark="阿里云定位appCode", Status=0, GroupCode="DEFAULT"},
                new SysConfig{Id=29, Name="Oauth用户登录的开关", Code="DILON_ENABLE_OAUTH_LOGIN", Value="true",SysFlag="Y", Remark="Oauth用户登录的开关", Status=0, GroupCode="OAUTH"},
                new SysConfig{Id=30, Name="Oauth码云登录ClientId", Code="DILON_OAUTH_GITEE_CLIENT_ID", Value="你的clientId",SysFlag="Y", Remark="Oauth码云登录ClientId", Status=0, GroupCode="OAUTH"},
                new SysConfig{Id=31, Name="Oauth码云登录ClientSecret", Code="DILON_OAUTH_GITEE_CLIENT_SECRET", Value="你的clientSecret",SysFlag="Y", Remark="Oauth码云登录ClientSecret", Status=0, GroupCode="OAUTH"},
                new SysConfig{Id=32, Name="Oauth码云登录回调地址", Code="DILON_OAUTH_GITEE_REDIRECT_URI", Value="http://127.0.0.1:5566/oauth/callback/gitee",SysFlag="Y", Remark="Oauth码云登录回调地址", Status=0, GroupCode="OAUTH"},
                new SysConfig{Id=33, Name="演示环境", Code="DILON_DEMO_ENV_FLAG", Value="false",SysFlag="Y", Remark="演示环境的开关,true-打开，false-关闭，如果演示环境开启，则只能读数据不能写数据", Status=0, GroupCode="DEFAULT"},
            };
        }
    }
}
