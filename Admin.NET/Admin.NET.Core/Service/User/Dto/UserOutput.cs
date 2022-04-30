namespace Admin.NET.Core.Service
{
    [NotTable]
    public class UserOutput : SysUser
    {
        /// <summary>
        /// 机构名称
        /// </summary>
        public string OrgName { get; set; }

        /// <summary>
        /// 职位名称
        /// </summary>
        public string PosName { get; set; }
    }
}