namespace Admin.NET.Core.Service;

public class EditColumnInput
{
    public string TableName { get; set; }

    public string OldName { get; set; }

    public string DbColumnName { get; set; }

    public string ColumnDescription { get; set; }
}