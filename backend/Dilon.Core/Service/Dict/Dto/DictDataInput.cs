using Furion.DataValidation;
using System.ComponentModel.DataAnnotations;

namespace Dilon.Core.Service
{
    /// <summary>
    /// 字典值参数
    /// </summary>
    public class DictDataInput : PageInputBase
    {
        /// <summary>
        /// 字典值Id
        /// </summary>
        public virtual long Id { get; set; }

        /// <summary>
        /// 字典类型Id
        /// </summary>
        public virtual long TypeId { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public virtual string Value { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public virtual int Sort { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }

        /// <summary>
        /// 状态（字典 0正常 1停用 2删除）
        /// </summary>
        public virtual int Status { get; set; }
    }

    public class QueryDictDataListInput
    {
        /// <summary>
        /// 字典类型Id
        /// </summary>
        [Required(ErrorMessage = "字典类型Id不能为空"), DataValidation(ValidationTypes.Numeric)]
        public long TypeId { get; set; }
    }

    public class AddDictDataInput : DictDataInput
    {
        /// <summary>
        /// 字典类型Id
        /// </summary>
        [Required(ErrorMessage = "字典类型Id不能为空"), DataValidation(ValidationTypes.Numeric)]
        public override long TypeId { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        [Required(ErrorMessage = "字典值不能为空")]
        public override string Value { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [Required(ErrorMessage = "字典值编码不能为空")]
        public override string Code { get; set; }
    }

    public class DeleteDictDataInput
    {
        /// <summary>
        /// 字典值Id
        /// </summary>
        [Required(ErrorMessage = "字典值Id不能为空"), DataValidation(ValidationTypes.Numeric)]
        public long Id { get; set; }
    }

    public class UpdateDictDataInput : AddDictDataInput
    {
        /// <summary>
        /// 字典值Id
        /// </summary>
        [Required(ErrorMessage = "字典值Id不能为空"), DataValidation(ValidationTypes.Numeric)]
        public override long Id { get; set; }
    }

    public class QueryDictDataInput : DeleteDictDataInput
    {

    }
}
