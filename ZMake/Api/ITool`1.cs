namespace ZMake.Api;

public interface ITool<T> : ITool
{
    Task<bool> Call(string workDir,
        Dictionary<string,string> environment,
        T argument);
}
