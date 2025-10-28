namespace ZMake.Api;

public interface ITool<T> : ITool where T : ToolArguments
{
    Task<bool> Call(string workDir,
        Dictionary<string,string> environment,
        T argument);
}
