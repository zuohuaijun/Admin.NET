using System.Collections.Generic;

namespace Admin.NET.Core.Service
{
    /// <summary>
    /// 授权用户机构
    /// </summary>
    public class UserOrgInput : BaseIdInput
    {
        /// <summary>
        /// 机构Id
        /// </summary>
        public long OrgId { get; set; }

        /// <summary>
        /// 机构Id列表
        /// </summary>
        public List<long> OrgIdList { get; set; }
    }
}