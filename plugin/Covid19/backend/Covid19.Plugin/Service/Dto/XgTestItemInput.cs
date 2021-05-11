using Admin.NET.Core;
using System.ComponentModel.DataAnnotations;

namespace Covid19.Plugin
{
    /// <summary>
    /// 核酸检测项目参数
    /// </summary>
    public class XgTestItemInput : PageInputBase
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        public virtual long Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// 类型（1非定期检测 2定期检测）
        /// </summary>
        public virtual int Type { get; set; }

        /// <summary>
        /// 适用层级(全省、全市、全县、本医院)
        /// </summary>
        public virtual string Area { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public virtual int Sort { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
    }

    public class AddXgTestItemInput : XgTestItemInput
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "项目名称不能为空")]
        public override string Name { get; set; }

        /// <summary>
        /// 项目类型
        /// </summary>
        [Required(ErrorMessage = "项目类型不能为空")]
        public override int Type { get; set; }

        /// <summary>
        /// 适用层级
        /// </summary>
        [Required(ErrorMessage = "适用层级不能为空")]
        public override string Area { get; set; }
    }

    public class DeleteXgTestItemInput : XgTestItemInput
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        [Required(ErrorMessage = "项目Id不能为空")]
        public override long Id { get; set; }
    }

    public class UpdateXgTestItemInput : AddXgTestItemInput
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        [Required(ErrorMessage = "项目Id不能为空")]
        public override long Id { get; set; }
    }

    public class QueryXgTestItemInput : DeleteXgTestItemInput
    {
    }


}
