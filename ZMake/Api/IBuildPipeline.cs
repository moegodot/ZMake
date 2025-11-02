namespace ZMake.Api;

public interface IBuildPipeline
{
    IEnumerable<IBuildChecker> Checkers { get; }

    Task Build(BuildConstant target);
}
