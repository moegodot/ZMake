using Semver;

namespace ZMake.Api;

public interface ITool
{
    string? ProgramPath { get; }

    ToolName Name { get; }

    ToolType Type { get; }

    Task<SemVersion?> GetVersionAsync();

    SemVersion? GetVersion();

    Task<bool> Call(string workDir,
        Dictionary<string,string> environment,
        IEnumerable<string> arguments);
}
