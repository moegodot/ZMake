namespace ZMake.Api.BuiltIn;

public sealed class BuildPipeline<T> : IBuildPipeline<T> where T : ToolArguments
{
    public List<IBuildChecker<T>> BuildCheckers { get; } = [];

    public List<IBuildTool<T>> BuildTools { get; } = [];

    public IEnumerable<IBuildChecker<T>> Checkers => BuildCheckers;

    public IEnumerable<IBuildTool<T>> Tools => BuildTools;

    public async Task Build(BuildConstant<T> target)
    {
        var changed = false;

        foreach (var checker in BuildCheckers)
        {
            changed = await checker.CheckChanged(target);

            if (changed)
            {
                break;
            }
        }

        if (!changed)
        {
            return;
        }

        await Task.WhenAll(BuildTools.Select((tool) => tool.Build(target)).ToArray());

        foreach (var checker in BuildCheckers)
        {
            await checker.Update(target);
        }
    }
}
