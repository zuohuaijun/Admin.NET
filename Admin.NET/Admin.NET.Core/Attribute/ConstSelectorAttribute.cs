namespace Admin.NET.Core;

/// <summary>
/// 枚举下拉框
/// </summary>
public class EnumSelectorAttribute : Attribute
{
    public EnumSelectorAttribute(string name)
    {
        Name = name;
    }
    public string Name { get; set; }
}