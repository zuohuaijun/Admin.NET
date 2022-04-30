namespace Admin.NET.Core.Service
{
    public class UserExtOrgPosOutput
    {
        /// <summary>
        /// 机构Id
        /// </summary>
        public long OrgId { get; set; }

        /// <summary>
        /// 机构编码
        /// </summary>
        public string OrgCode { get; set; }

        /// <summary>
        /// 机构名称
        /// </summary>
        public string OrgName { get; set; }

        /// <summary>
        /// 职位Id
        /// </summary>
        public long PosId { get; set; }

        /// <summary>
        /// 职位编码
        /// </summary>
        public string PosCode { get; set; }

        /// <summary>
        /// 职位名称
        /// </summary>
        public string PosName { get; set; }
    }
}