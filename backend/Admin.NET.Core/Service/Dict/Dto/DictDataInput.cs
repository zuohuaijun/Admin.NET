using Furion.DataValidation;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Core.Service
{
    /// <summary>
    /// 字典值参数
    /// </summary>
    public class DictDataPageInput : PageInputBase
    {
        /// <summary>
        /// 字典类型Id
        /// </summary>
        public long TypeId { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
    }

    public class QueryDictDataListInput
    {
        /// <summary>
        /// 字典类型Id
        /// </summary>
        [Required(ErrorMessage = "字典类型Id不能为空"), DataValidation(ValidationTypes.Numeric)]
        public long TypeId { get; set; }
    }

    public class AddDictDataInput
    {
        /// <summary>
        /// 字典类型Id
        /// </summary>
        [Required(ErrorMessage = "字典类型Id不能为空"), DataValidation(ValidationTypes.Numeric)]
        public long TypeId { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        [Required(ErrorMessage = "字典值不能为空")]
        public string Value { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [Required(ErrorMessage = "字典值编码不能为空")]
        public string Code { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 状态（字典 0正常 1停用 2删除）
        /// </summary>
        public CommonStatus Status { get; set; }
    }

    public class DeleteDictDataInput : BaseId
    {
    }

    public class UpdateDictDataInput
    {
        /// <summary>
        /// 字典值Id
        /// </summary>
        [Required(ErrorMessage = "字典值Id不能为空"), DataValidation(ValidationTypes.Numeric)]
        public long Id { get; set; }

        /// <summary>
        /// 字典类型Id
        /// </summary>
        [Required(ErrorMessage = "字典类型Id不能为空"), DataValidation(ValidationTypes.Numeric)]
        public long TypeId { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        [Required(ErrorMessage = "字典值不能为空")]
        public string Value { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [Required(ErrorMessage = "字典值编码不能为空")]
        public string Code { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 状态（字典 0正常 1停用 2删除）
        /// </summary>
        public CommonStatus Status { get; set; }
    }

    public class ChageStateDictDataInput : BaseId
    {
        /// <summary>
        /// 状态（字典 0正常 1停用 2删除）
        /// </summary>
        public CommonStatus Status { get; set; }
    }

    public class QueryDictDataInput : BaseId
    {
    }
}