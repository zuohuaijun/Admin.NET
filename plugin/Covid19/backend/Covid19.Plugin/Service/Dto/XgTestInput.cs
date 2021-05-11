using Admin.NET.Core;
using System;
using System.ComponentModel.DataAnnotations;

namespace Covid19.Plugin
{
    /// <summary>
    /// 核酸检测参数
    /// </summary>
    public class XgTestInput : PageInputBase
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual long Id { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public virtual Gender Sex { get; set; }

        /// <summary>
        /// 证件号码
        /// </summary>
        public virtual string IdNumber { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public virtual string Phone { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public virtual DateTimeOffset? Birthday { get; set; }

        /// <summary>
        /// 站点名称
        /// </summary>
        public virtual string SiteId { get; set; }

        /// <summary>
        /// 采集日期
        /// </summary>
        public virtual DateTimeOffset CollectionTime { get; set; }

        /// <summary>
        /// 采集编号
        /// </summary>
        public virtual string Number { get; set; }

        /// <summary>
        /// 新冠ORFlab(1阴性 2阳性)
        /// </summary>
        public virtual int XgOrflab { get; set; }

        /// <summary>
        /// 新冠N(1阴性 2阳性)
        /// </summary>
        public virtual int XgN { get; set; }

        /// <summary>
        /// 抗体IgG
        /// </summary>
        public virtual string IgG { get; set; }

        /// <summary>
        /// 抗体IgM
        /// </summary>
        public virtual string IgM { get; set; }

        /// <summary>
        /// 检验医生
        /// </summary>
        public virtual string TestDoctor { get; set; }

        /// <summary>
        /// 检验时间
        /// </summary>
        public virtual DateTimeOffset? TestTime { get; set; }

        /// <summary>
        /// 审核医生
        /// </summary>
        public virtual string AuditDoctor { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public virtual DateTimeOffset? AuditTime { get; set; }
    }

    public class DeleteXgTestInput : XgTestInput
    {
        /// <summary>
        /// Id
        /// </summary>
        [Required(ErrorMessage = "Id不能为空")]
        public override long Id { get; set; }
    }

    public class UpdateXgTestInput : DeleteXgTestInput
    {
    }

    public class QueryXgTestInput : DeleteXgTestInput
    {
    }

    public class IdsInput
    {
        public long[] Ids { get; set; }
    }
}
