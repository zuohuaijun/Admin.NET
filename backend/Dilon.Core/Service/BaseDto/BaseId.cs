using Furion.DataValidation;
using System.ComponentModel.DataAnnotations;

namespace Dilon.Core.Service
{
    public class BaseId
    {
        /// <summary>
        /// 应用Id
        /// </summary>
        [Required(ErrorMessage = "Id不能为空")]
        [DataValidation(ValidationTypes.Numeric)]
        public long Id { get; set; }
    }
}
