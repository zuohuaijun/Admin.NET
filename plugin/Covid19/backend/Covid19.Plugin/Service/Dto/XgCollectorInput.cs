using Admin.NET.Core;
using System;
using System.ComponentModel.DataAnnotations;

namespace Covid19.Plugin
{
    /// <summary>
    /// 样本采集人员参数
    /// </summary>
    public class XgCollectorInput : PageInputBase
    {
        /// <summary>
        /// 样本Id
        /// </summary>
        public virtual long Id { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 证件号码
        /// </summary>
        public virtual string IdNumber { get; set; }

        /// <summary>
        /// 证件类型（1身份证 2护照 3其他）
        /// </summary>
        public virtual int IdType { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public virtual string Phone { get; set; }

        /// <summary>
        /// 性别-男_1、女_2
        /// </summary>
        public virtual int Sex { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public virtual DateTimeOffset? Birthday { get; set; }

        /// <summary>
        /// 住址
        /// </summary>
        public virtual string Address { get; set; }

        /// <summary>
        /// 采集日期
        /// </summary>
        public virtual DateTimeOffset CollectionTime { get; set; }

        /// <summary>
        /// 采集编号
        /// </summary>
        public virtual string Number { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public virtual string SiteId { get; set; }

        /// <summary>
        /// 站点名称
        /// </summary>
        public virtual string SiteName { get; set; }

        /// <summary>
        /// 社会信用码
        /// </summary>
        public virtual string SocialCode { get; set; }

        /// <summary>
        /// 工作单位
        /// </summary>
        public virtual string WorkUnit { get; set; }

        /// <summary>
        /// 工作岗位
        /// </summary>
        public virtual string Job { get; set; }

        /// <summary>
        /// 疫情重点地区
        /// </summary>
        public virtual string EpidemicArea { get; set; }

        /// <summary>
        /// 人员类别编码
        /// </summary>
        public virtual string TypeCode { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
    }

    public class AddXgCollectorInput : XgCollectorInput
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "人员名称不能为空")]
        public override string Name { get; set; }

        /// <summary>
        /// 证件号码
        /// </summary>
        [Required(ErrorMessage = "证件号码不能为空")]
        public override string IdNumber { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [Required(ErrorMessage = "电话不能为空")]
        public override string Phone { get; set; }
    }

    public class DeleteXgCollectorInput : XgCollectorInput
    {
        /// <summary>
        /// 样本Id
        /// </summary>
        [Required(ErrorMessage = "样本Id不能为空")]
        public override long Id { get; set; }
    }

    public class UpdateXgCollectorInput : AddXgCollectorInput
    {
        /// <summary>
        /// 样本Id
        /// </summary>
        [Required(ErrorMessage = "样本Id不能为空")]
        public override long Id { get; set; }
    }

    public class QueryXgCollectorInput : DeleteXgCollectorInput
    {
    }
}
