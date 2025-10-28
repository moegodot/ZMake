namespace ZMake.Api;

public interface IBuildPipeline<T> where T : ToolArguments
{
    IEnumerable<IBuildChecker<T>> Checkers { get; }

    IEnumerable<IBuildTool<T>> Tools { get; }

    Task Build(BuildConstant<T> target);
}
