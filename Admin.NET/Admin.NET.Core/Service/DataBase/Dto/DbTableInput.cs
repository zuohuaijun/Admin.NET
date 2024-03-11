// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core.Service;

public class DbTableInput
{
    public string ConfigId { get; set; }

    public string TableName { get; set; }

    public string Description { get; set; }

    public List<DbColumnInput> DbColumnInfoList { get; set; }
}

public class UpdateDbTableInput
{
    public string ConfigId { get; set; }

    public string TableName { get; set; }

    public string OldTableName { get; set; }

    public string Description { get; set; }
}

public class DeleteDbTableInput
{
    public string ConfigId { get; set; }

    public string TableName { get; set; }
}