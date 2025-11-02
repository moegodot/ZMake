namespace ZMake.Api;

public class DotnetTargetEmitter : ITargetEmitter
{
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

    public DotnetTargetEmitter(Name targetName, ToolChain toolChain)
    {
        CacheKey = $"dotnet-project-{targetName}";
        ToolChain = toolChain;
        TargetName = targetName;
    }

    public TargetBuilder AddDotnetTarget(
        IEnumerable<string> @in,
        IEnumerable<string> @out,
        DotnetArguments arguments, string? withName = null)
    {
        return new TargetBuilder()
            .WithName(TargetName.Append(
                withName
            ?? arguments.Command
            ?? throw new ArgumentNullException(nameof(arguments),
            "require withName is not null when DotnetArguments.Command is not null")))
            .WithBuildActionWriteTo(_publicTargets)
            .WithBuildToolCall(ToolChain, CSharp.Dotnet, @in, @out, arguments);
    }

    public Name EmitterName { get; } = Name.Create(Version.V1V0V0, "target_emitter.dotnet");

    private readonly Dictionary<Name, Target> _publicTargets = [];

    public IReadOnlyDictionary<Name, Target> PublicTargets => _publicTargets.AsReadOnly();
}
