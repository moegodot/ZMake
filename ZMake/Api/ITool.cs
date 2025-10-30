using Semver;

namespace ZMake.Api;

public interface ITool : IGetHashCode128
{
    string? ProgramPath { get; }

    ToolName Name { get; }

    ToolType Type { get; }

    Task<SemVersion?> GetVersionAsync();

    SemVersion? GetVersion();

    Task<bool> Call(
        IEnumerable<string> arguments,
        string? workDir = null,
        IReadOnlyDictionary<string, string>? environment = null);
}
