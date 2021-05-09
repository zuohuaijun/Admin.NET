using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Core.Service
{
    /// <summary>
    /// 枚举输入参数
    /// </summary>
    public class EnumDataInput
    {
        /// <summary>
        /// 枚举类型名称
        /// </summary>
        [Required(ErrorMessage = "枚举类型不能为空")]
        public string EnumName { get; set; }
    }

    public class QueryEnumDataInput
    {
        /// <summary>
        /// 实体名称
        /// </summary>
        [Required(ErrorMessage = "实体名称不能为空")]
        public string EntityName { get; set; }

        /// <summary>
        /// 字段名称
        /// </summary>
        [Required(ErrorMessage = "字段名称不能为空")]
        public string FieldName { get; set; }
    }
}