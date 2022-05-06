namespace Admin.NET.Core
{
    /// <summary>
    /// 通用常量
    /// </summary>
    public class CommonConst
    {
        /// <summary>
        /// 默认密码
        /// </summary>
        public const string SysPassword = "123456";

        /// <summary>
        /// 系统管理员角色编码
        /// </summary>
        public const string SysAdminRoleCode = "SysAdminRoleCode";

        /// <summary>
        /// 演示环境开关配置
        /// </summary>
        public const string SysDemoEnv = "SysDemoEnv";

        /// <summary>
        /// 验证码开关配置
        /// </summary>
        public const string SysCaptchaFlag = "SysCaptchaFlag";

        /// <summary>
        /// 开启操作日志配置
        /// </summary>
        public const string SysOpLogFlag = "SysOpLog";

        /// <summary>
        /// 实体所在程序集-代码生成
        /// </summary>
        public static string[] EntityAssemblyName = new string[] { "Admin.NET.Core", "Admin.NET.Application" };
    }
}