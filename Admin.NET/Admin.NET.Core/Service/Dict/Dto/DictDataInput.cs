using Furion.DataValidation;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Core.Service
{
    public class DictDataInput : BaseIdInput
    {
    }

    public class GetDataDictDataInput
    {
        /// <summary>
        /// 字典类型Id
        /// </summary>
        [Required(ErrorMessage = "字典类型Id不能为空"), DataValidation(ValidationTypes.Numeric)]
        public long DictTypeId { get; set; }
    }

    /// <summary>
    /// 字典值分页
    /// </summary>
    public class PageDictDataInput : BasePageInput
    {
        /// <summary>
        /// 字典类型Id
        /// </summary>
        public long DictTypeId { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
    }

    public class AddDictDataInput
    {
        /// <summary>
        /// 字典类型Id
        /// </summary>
        public long DictTypeId { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
    }

    public class UpdateDictDataInput : BaseIdInput
    {
        /// <summary>
        /// 字典类型Id
        /// </summary>
        public long DictTypeId { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
    }

    public class DeleteDictDataInput : BaseIdInput
    {
    }

    public class ChageStatusDictDataInput : BaseIdInput
    {
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
    }
}