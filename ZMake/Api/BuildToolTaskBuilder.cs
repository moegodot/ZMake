namespace ZMake.Api;

public class BuildToolTaskBuilder<T> : TaskBuilder where T : ToolArguments
{
    public IBuildTool<T>? Tool { get; set; } = null;
    public List<string> Sources { get; set; } = [];
    public List<string> Outputs { get; set; } = [];
    public Dictionary<string, string>? Environments { get; set; } = [];
    public T Options { get; set; } = default!;
    public string? WorkDir { get; set; } = null;

    public TaskType TaskType { get; set; } = TaskType.LongRunning;

    public BuildToolTaskBuilder(IContext context) : this(context.TaskEngine)
    {
    }

    public BuildToolTaskBuilder(TaskEngine engine) : base(engine)
    {
    }

    public BuildToolTaskBuilder<T> WithTool(IBuildTool<T> tool)
    {
        Tool = tool;
        return this;
    }

    public BuildToolTaskBuilder<T> AddSources(params IEnumerable<string> sources)
    {
        Sources.AddRange(sources);
        return this;
    }
    public BuildToolTaskBuilder<T> AddOutputs(params IEnumerable<string> outputs)
    {
        Outputs.AddRange(outputs);
        return this;
    }

    public BuildToolTaskBuilder<T> WithEnvironments(Dictionary<string, string> environments)
    {
        Environments = environments;
        return this;
    }

    public BuildToolTaskBuilder<T> WithOptions(T options)
    {
        Options = options;
        return this;
    }

    public BuildToolTaskBuilder<T> WithWorkDir(string workDir)
    {
        WorkDir = workDir;
        return this;
    }

    public BuildToolTaskBuilder<T> WithTaskType(TaskType taskType)
    {
        TaskType = taskType;
        return this;
    }

    public override Func<Task> Build()
    {
        if (Tool is null)
        {
            throw new InvalidOperationException("Tool is not set");
        }

        var func = Func;
        Func = () => Task.CompletedTask;
        ContinueWith(async () =>
        {
            await Tool.Build(Sources, Outputs, Options, WorkDir, Environments);
        });
        ContinueWith(func);

        return base.Build();
    }
}
