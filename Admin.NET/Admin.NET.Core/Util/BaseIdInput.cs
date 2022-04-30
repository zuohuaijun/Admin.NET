using Furion.DataValidation;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Core
{
    /// <summary>
    /// 主键Id输入参数
    /// </summary>
    public class BaseIdInput
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Required(ErrorMessage = "Id不能为空")]
        [DataValidation(ValidationTypes.Numeric)]
        public virtual long Id { get; set; }
    }
}