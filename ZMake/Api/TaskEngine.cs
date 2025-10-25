using Microsoft.Extensions.Logging;

namespace ZMake.Api;

public sealed class TaskEngine
{
    private List<Task> Tasks = new();

    public void AddTaskToRun(TaskType? type,Task task)
    {
        task.Start();
        Tasks.Add(task);
    }

    public void Join()
    {
        foreach (var task in Tasks)
        {
            task.Wait();
        }
        Tasks.Clear();
    }
}
