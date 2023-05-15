namespace Admin.NET.Core.Service;

public class PagePrintInput : BasePageInput
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

public class AddPrintInput : SysPrint
{
    /// <summary>
    /// 名称
    /// </summary>
    [Required(ErrorMessage = "模板名称不能为空")]
    public override string Name { get; set; }
}

public class UpdatePrintInput : AddPrintInput
{
}

public class DeletePrintInput : BaseIdInput
{
}