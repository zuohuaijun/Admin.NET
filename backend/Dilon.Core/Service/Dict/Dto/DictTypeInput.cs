using System.ComponentModel.DataAnnotations;

namespace Dilon.Core.Service
{
    /// <summary>
    /// 字典类型参数
    /// </summary>
    public class DictTypeInput : PageInputBase
    {
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }

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
        public virtual CommonStatus Status { get; set; }
    }

    public class AddDictTypeInput : DictTypeInput
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "字典类型名称不能为空")]
        public override string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [Required(ErrorMessage = "字典类型编码不能为空")]
        public override string Code { get; set; }
    }

    public class DeleteDictTypeInput
    {
        /// <summary>
        /// 编号Id
        /// </summary>
        [Required(ErrorMessage = "字典类型Id不能为空")]
        public long Id { get; set; }
    }

    public class UpdateDictTypeInput : AddDictTypeInput
    {
        /// <summary>
        /// Id
        /// </summary>
        [Required(ErrorMessage = "字典类型Id不能为空")]
        public long Id { get; set; }
    }

    public class ChangeStateDictTypeInput : DictTypeInput
    {
        /// <summary>
        /// Id
        /// </summary>
        [Required(ErrorMessage = "字典类型Id不能为空")]
        public long Id { get; set; }
    }

    public class DropDownDictTypeInput
    {
        /// <summary>
        /// 编码
        /// </summary>
        [Required(ErrorMessage = "字典类型编码不能为空")]
        public string Code { get; set; }
    }

    public class QueryDictTypeInfoInput : DeleteDictTypeInput
    {
    }
}