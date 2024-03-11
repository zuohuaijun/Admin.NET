// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core.Service;

public class DictDataInput : BaseIdInput
{
    /// <summary>
    /// 状态
    /// </summary>
    public StatusEnum Status { get; set; }
}

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

public class AddDictDataInput : SysDictData
{
}

public class UpdateDictDataInput : AddDictDataInput
{
}

public class DeleteDictDataInput : BaseIdInput
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

public class QueryDictDataInput
{
    /// <summary>
    /// 编码
    /// </summary>
    [Required(ErrorMessage = "字典唯一编码不能为空")]
    public string Code { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public int? Status { get; set; }
}