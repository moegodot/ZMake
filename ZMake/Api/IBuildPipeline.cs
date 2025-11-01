namespace ZMake.Api;

public interface IBuildPipeline<T> where T : ToolArguments
{
    IEnumerable<IBuildChecker<T>> Checkers { get; }

    Task Build(BuildConstant<T> target);
}
