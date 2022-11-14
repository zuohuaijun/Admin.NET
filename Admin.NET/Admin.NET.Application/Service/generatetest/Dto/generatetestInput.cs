using System;
using Admin.NET.Core;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Admin.NET.Application
{
    /// <summary>
    /// GenerateTest输入参数
    /// </summary>
    public class generatetestInput : BasePageInput
    {
        /// <summary>
        /// 编码
        /// </summary>
        public virtual string Code { get; set; }
        
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }
        
        /// <summary>
        /// 价格
        /// </summary>
        public virtual decimal Price { get; set; }
        
        /// <summary>
        /// 过期日期
        /// </summary>
        public virtual DateTime ExpireDate { get; set; }
        
        /// <summary>
        /// 状态
        /// </summary>
        public virtual bool Status { get; set; }
        
    }

    public class AddgeneratetestInput : generatetestInput
    {
        /// <summary>
        /// 编码
        /// </summary>
        [Required(ErrorMessage = "编码不能为空")]
        public override string Code { get; set; }
        
        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "名称不能为空")]
        public override string Name { get; set; }
        
    }

    public class DeletegeneratetestInput : BaseIdInput
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Required(ErrorMessage = "主键Id不能为空")]
        public long Id { get; set; }
        
    }

    public class UpdategeneratetestInput : generatetestInput
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Required(ErrorMessage = "主键Id不能为空")]
        public long Id { get; set; }
        
    }

    public class QueryegeneratetestInput : DeletegeneratetestInput
    {

    }
}
