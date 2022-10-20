namespace Admin.NET.Core.Service;

public class PageTimerInput : BasePageInput
{
    /// <summary>
    /// 任务名称
    /// </summary>
    public string TimerName { get; set; }
}

[NotTable]
public class AddTimerInput : SysTimer
{
    /// <summary>
    /// 任务名称
    /// </summary>
    [Required(ErrorMessage = "任务名称不能为空")]
    public override string TimerName { get; set; }
}

[NotTable]
public class UpdateTimerInput : AddTimerInput
{
}

public class DeleteTimerInput : BaseIdInput
{
}

[NotTable]
public class StopTimerInput : AddTimerInput
{
}

public class SetTimerStatusInput
{
    /// <summary>
    /// 任务名称
    /// </summary>
    [Required(ErrorMessage = "任务名称不能为空")]
    public string TimerName { get; set; }

    /// <summary>
    /// 任务状态
    /// </summary>
    public SpareTimeStatus Status { get; set; } = SpareTimeStatus.Stopped;
}