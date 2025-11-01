namespace ZMake.Api;

public class TaskBuilder
{
    public bool IsBuilt { get; private set; } = false;
    public Func<Task> Func { get; set; }
    public TaskEngine Engine { get; }

    public TaskBuilder(IContext context) : this(context.TaskEngine)
    {
    }

    public TaskBuilder(TaskEngine engine)
    {
        Engine = engine;

        Func = () => Task.CompletedTask;
    }

    public virtual Func<Task> Build()
    {
        CheckBuilt();
        IsBuilt = true;
        return Func;
    }

    public TaskBuilder ContinueWith(Action task, TaskType type = TaskType.Default)
    {
        return ContinueWith((_) => task(),type);
    }

    public TaskBuilder ContinueWith(Func<Task> task,TaskType type = TaskType.Default)
    {
        return ContinueWith((_) => task(),type);
    }

    public TaskBuilder ContinueWith(Action<Task> task,TaskType type = TaskType.Default)
    {
        return ContinueWith((result) =>
        {
            task(result);
            return Task.CompletedTask;
        },type);
    }

    public TaskBuilder ContinueWith(Func<Task, Task> task,TaskType type = TaskType.Default)
    {
        CheckBuilt();
        var func = Func;
        Func = async () =>
        {
            var preTask = func();
            await preTask;
            await Engine[type].StartNew(async () =>
            {
                await task(preTask);
            }).Unwrap();
        };
        return this;
    }

    private void CheckBuilt()
    {
        if (IsBuilt)
        {
            throw new InvalidOperationException("The task has been built");
        }
    }

    public void GetBuildResult(out Func<Task> func)
    {
        if (IsBuilt)
        {
            func = Func;
            return;
        }
        func = Build();
    }

    public TaskBuilder ContinueWith(TaskBuilder next)
    {
        CheckBuilt();

        var func = Func;
        Func = async () =>
        {
            await func();
            await next.Build()();
        };

        Build();

        return next;
    }
}

