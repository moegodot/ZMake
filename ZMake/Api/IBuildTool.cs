namespace ZMake.Api;

public interface IBuildTool<T> : ITool where T : ToolArguments
{
    Task<bool> Build(
        IEnumerable<string> @in,
        IEnumerable<string> @out,
        T arguments,
        string? workDir = null,
        IReadOnlyDictionary<string, string>? environment = null);

    Task<bool> Build(BuildConstant build)
    {
        return Build(build.Sources, build.Outputs, build.Options.OfType<T>().FirstOrDefault()
            ?? throw new InvalidOperationException($"Failed to find options of {typeof(T)} in BuildConstants.Options"),
            build.WorkDir, build.Environments);
    }
}
