// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

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