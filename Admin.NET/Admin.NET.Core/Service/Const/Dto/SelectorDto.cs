namespace Admin.NET.Core.Service;

public class SelectorDto
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 编码
    /// </summary>
    public dynamic Code { get; set; }

    /// <summary>
    /// 扩展字段
    /// </summary>
    public dynamic Data { get; set; }
}