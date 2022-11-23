namespace Admin.NET.Core.Service;

public class JobInput
{
    /// <summary>
    /// 作业任务名称
    /// </summary>
    [Required(ErrorMessage = "作业任务名称不能为空")]
    public string Name { get; set; }
}