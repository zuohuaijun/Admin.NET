namespace Admin.NET.Core.Service;

public class EditTableInput
{
    public string ConfigId { get; set; }

    public string Name { get; set; }

    public string OldName { get; set; }

    public string Description { get; set; }
}

public class DeleteTableInput
{
    public string ConfigId { get; set; }

    public string Name { get; set; }
}