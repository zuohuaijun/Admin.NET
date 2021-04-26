namespace Dilon.Core.Service
{
    /// <summary>
    /// 登录用户角色参数
    /// </summary>
    public class RoleOutput
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }
}