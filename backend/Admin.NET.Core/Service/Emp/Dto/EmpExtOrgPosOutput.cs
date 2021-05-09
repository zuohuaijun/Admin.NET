namespace Admin.NET.Core.Service
{
    /// <summary>
    /// 附属机构和职位参数
    /// </summary>
    public class EmpExtOrgPosOutput
    {
        /// <summary>
        /// 附属机构id
        /// </summary>
        public long OrgId { get; set; }

        /// <summary>
        /// 附属机构编码
        /// </summary>
        public string OrgCode { get; set; }

        /// <summary>
        /// 附属机构名称
        /// </summary>
        public string OrgName { get; set; }

        /// <summary>
        /// 附属职位id
        /// </summary>
        public long PosId { get; set; }

        /// <summary>
        /// 附属职位编码
        /// </summary>
        public string PosCode { get; set; }

        /// <summary>
        /// 附属职位名称
        /// </summary>
        public string PosName { get; set; }
    }
}