namespace ZMake.Api;

public interface IBuildTool<T> : ITool where T : ToolArguments
{
    Task<bool> Build(
        IEnumerable<string> @in,
        IEnumerable<string> @out,
        string workDir,
        Dictionary<string,string> environment,
        T arguments);

    Task<bool> Build(BuildConstant<T> build)
    {
        return Build(build.Sources, build.Outputs, build.WorkDir, build.Environments, build.Options);
    }
}
