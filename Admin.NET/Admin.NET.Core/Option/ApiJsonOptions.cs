using Admin.NET.Core.Service.ApiJson;

namespace Admin.NET.Core;

/// <summary>
/// ApiJson配置选项
/// </summary>
public sealed class ApiJsonOptions : IConfigurableOptions
{
    /// <summary>
    /// ApiJson集合RoleList
    /// </summary>
    public List<Role> RoleList { get; set; }
    //public Tablempper Tablempper { get; set; }
}

//public sealed class Tablempper
//{
//    public string User { get; set;}
//    public string Org { get; set;}
//}