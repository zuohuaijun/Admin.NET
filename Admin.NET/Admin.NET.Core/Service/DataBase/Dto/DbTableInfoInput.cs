namespace Admin.NET.Core.Service;

public class DbTableInfoInput
{
    public string Name { get; set; }

    public string Description { get; set; }

    public List<DbColumnInfoInput> DbColumnInfoList { get; set; }
}