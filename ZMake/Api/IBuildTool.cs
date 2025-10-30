namespace ZMake.Api;

public interface IBuildTool<T> : ITool where T : ToolArguments
{
    Task<bool> Build(
        IEnumerable<string> @in,
        IEnumerable<string> @out,
        T arguments,
        string? workDir = null,
        IReadOnlyDictionary<string, string>? environment = null);

    Task<bool> Build(BuildConstant<T> build)
    {
        return Build(build.Sources, build.Outputs, build.Options, build.WorkDir, build.Environments);
    }
}
