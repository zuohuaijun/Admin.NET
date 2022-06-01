namespace Admin.NET.Core.Service;

public class DictTypeInput : BaseIdInput
{
}

/// <summary>
/// 字典类型分页
/// </summary>
public class PageDictTypeInput : BasePageInput
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 编码
    /// </summary>
    public string Code { get; set; }
}

public class AddDictTypeInput
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

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

public class UpdateDictTypeInput : BaseIdInput
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

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

public class DeleteDictTypeInput : BaseIdInput
{
}

public class GetDataDictTypeInput
{
    /// <summary>
    /// 编码
    /// </summary>
    [Required(ErrorMessage = "字典类型编码不能为空")]
    public string Code { get; set; }
}

public class ChangeStatusDictTypeInput : BaseIdInput
{
    /// <summary>
    /// 状态
    /// </summary>
    public int Status { get; set; }
}