using System.Collections.ObjectModel;

namespace ZMake.Api.BuiltIn;

public class DotnetTargetEmitter : ITargetEmitter
{
    public string ProjectFilePath { get; }
    public string CacheKey { get; }
    public Name TargetName { get; }
    public ToolChain ToolChain { get; }

    public Memory<byte> GetCache()
    {
        return Array.Empty<byte>();
    }

    public bool Restore(Memory<byte> cachedData)
    {
        return cachedData.Length == 0;
    }

    public DotnetTargetEmitter(Name targetName, string projectFilePath, ToolChain toolChain)
    {
        ProjectFilePath = projectFilePath;
        CacheKey = $"dotnet-project-{projectFilePath}";
        ToolChain = toolChain;
        TargetName = targetName;
    }

    public TargetBuilder AddDotnetTarget(DotnetArguments arguments, string withName)
    {
        return new TargetBuilder()
            .WithName(TargetName.Append(withName))
            .WithBuildActionWriteTo(_publicTargets)
            .WithBuildToolCall(ToolChain, CSharp.Dotnet, [ProjectFilePath], [], arguments);
    }

    public Name EmitterName { get; } = Name.Create(Version.V1V0V0, "target_emitter.dotnet");

    private readonly Dictionary<Name, Target> _publicTargets = [];

    public IReadOnlyDictionary<Name, Target> PublicTargets => _publicTargets.AsReadOnly();
}
