namespace ZMake.Api;

public static class TaskBuilderExtension
{

    public static TaskBuilder AddTaskBuilder(this TaskBuilder builder)
    {
        return builder.ContinueWith(new TaskBuilder(builder.Engine));
    }

    public static BuildToolTaskBuilder<T> AddToolBuild<T>(this TaskBuilder builder, IBuildTool<T> tool)
        where T : ToolArguments
    {
        return (BuildToolTaskBuilder<T>)
            builder.ContinueWith(new BuildToolTaskBuilder<T>(builder.Engine).WithTool(tool));
    }
}
