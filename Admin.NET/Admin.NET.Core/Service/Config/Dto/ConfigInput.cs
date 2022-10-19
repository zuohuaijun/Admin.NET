namespace Admin.NET.Core.Service;

public class ConfigInput : BaseIdInput
{
}

public class PageConfigInput : BasePageInput
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

[NotTable]
public class AddConfigInput : SysConfig
{
}

[NotTable]
public class UpdateConfigInput : AddConfigInput
{
}

public class DeleteConfigInput : BaseIdInput
{
}