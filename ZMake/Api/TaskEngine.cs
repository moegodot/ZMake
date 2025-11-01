using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace ZMake.Api;

public sealed class TaskEngine
{
    public TaskFactory Default { get; } = new(TaskScheduler.Default);

    public TaskFactory IoBound { get; } = new(
        TaskCreationOptions.RunContinuationsAsynchronously,
        TaskContinuationOptions.None);

    public TaskFactory LongRunning { get; } =
        new(TaskCreationOptions.LongRunning,
            TaskContinuationOptions.LongRunning);

    public TaskFactory this[TaskType type]
    {
        get
        {
            return type switch
            {
                TaskType.Default => Default,
                TaskType.IoBound => IoBound,
                TaskType.LongRunning => LongRunning,
                _ => throw new UnreachableException(),
            };
        }
    }
}
