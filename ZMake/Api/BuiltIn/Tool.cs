using System.Diagnostics;
using Semver;

namespace ZMake.Api.BuiltIn;

public class Tool : ITool
{
    private bool _gotVersion = false;

    private SemVersion? _version = null;

    public string? ProgramPath { get; }
    public ToolName Name { get; }
    public ToolType Type { get; }

    public async Task<SemVersion?> GetVersionAsync()
    {
        if (_gotVersion || ProgramPath is null)
        {
            return _version;
        }

        _version = await VersionExtractor.TryGetVersionOfProgram(ProgramPath);

        _gotVersion = true;

        return _version;
    }

    public SemVersion? GetVersion()
    {
        var version = GetVersionAsync();
        version.ConfigureAwait(false);
        version.Wait();
        return version.Result;
    }

    public Tool(string program,ToolName name,ToolType type)
    {
        ProgramPath = program;
        Name = name;
        Type = type;
    }

    public async Task<bool> Call(
        string workDir,
        Dictionary<string, string> environment,
        IEnumerable<string> arguments)
    {
        Process process = new();

        process.StartInfo.UseShellExecute = false;
        process.StartInfo.Environment.Clear();
        process.StartInfo.FileName = ProgramPath ?? string.Empty;
        process.StartInfo.WorkingDirectory = workDir;

        foreach (var argument in arguments)
        {
            process.StartInfo.ArgumentList.Add(argument);
        }

        foreach(var env in environment){
            process.StartInfo.Environment.Add(env.Key,env.Value);
        }

        if (!process.Start())
        {
            throw new InvalidOperationException("Failed to start process");
        }

        await process.WaitForExitAsync();

        return process.ExitCode == 0;
    }

    public override string ToString()
    {
        return $"{ProgramPath} [version {GetVersionAsync()}]";
    }

    public override bool Equals(object? obj)
    {
        return obj is ITool tool &&
               (tool.ProgramPath?.Equals(ProgramPath))
               .GetValueOrDefault(ProgramPath is null);
    }

    public override int GetHashCode()
    {
        return ProgramPath?.GetHashCode() ?? string.Empty.GetHashCode();
    }

    public UInt128 GetHashCode128()
    {
        return HashCode128.Get(ProgramPath ?? string.Empty);
    }
}
