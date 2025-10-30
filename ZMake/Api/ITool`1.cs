namespace ZMake.Api;

public interface ITool<T> : ITool where T : ToolArguments
{
    Task<bool> Call(
        T arguments,
        string? workDir = null,
        IReadOnlyDictionary<string, string>? environment = null
        );
}
