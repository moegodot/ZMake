namespace ZMake.Api;

public interface IBuildTool<T> : ITool
{
    Task<bool> Build(
        IEnumerable<string> @in,
        IEnumerable<string> @out,
        string workDir,
        Dictionary<string,string> environment,
        T arguments);
}
