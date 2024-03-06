// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core.Service;

/// <summary>
/// 数据库表
/// </summary>
public class TableOutput
{
    /// <summary>
    /// 库定位器名
    /// </summary>
    public string ConfigId { get; set; }

    /// <summary>
    /// 表名（字母形式的）
    /// </summary>
    public string TableName { get; set; }

    /// <summary>
    /// 实体名称
    /// </summary>
    public string EntityName { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public string CreateTime { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public string UpdateTime { get; set; }

    /// <summary>
    /// 表名称描述（功能名）
    /// </summary>
    public string TableComment { get; set; }
}