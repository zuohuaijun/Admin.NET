namespace Admin.NET.Core.Service
{
    /// <summary>
    /// 员工职位参数
    /// </summary>
    public class EmpPosOutput
    {
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