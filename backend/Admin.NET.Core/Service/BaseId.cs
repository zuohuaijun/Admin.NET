using Furion.DataValidation;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Core.Service
{
    /// <summary>
    /// 主键Id映射DTO
    /// </summary>
    public class BaseId
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Required(ErrorMessage = "Id不能为空")]
        [DataValidation(ValidationTypes.Numeric)]
        public long Id { get; set; }
    }
}