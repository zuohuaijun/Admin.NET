namespace Admin.NET.Core.Service;

public class DbTableInfoInput
{
    public string ConfigId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public List<DbColumnInfoInput> DbColumnInfoList { get; set; }
}