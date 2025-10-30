using System.Collections.Frozen;

namespace ZMake.Api;

public sealed class ToolChainBuilder
{

    public Dictionary<ToolType, List<ITool>> Tools { get; set; } = [];

    public FileFinderBuilder BinaryFinder { get; set; }

    public Architecture NativeArchitecture { get; set; }

    public Architecture TargetArchitecture { get; set; }

    public Dictionary<string, string> Environments { get; set; }

    public ITool? this[ToolType toolType] => Tools[toolType]?.FirstOrDefault();

    private ToolChainBuilder(
        FileFinderBuilder binaryFinder,
        Architecture nativeArchitecture,
        Architecture targetArchitecture,
        Dictionary<string, string> environments)
    {
        BinaryFinder = binaryFinder;
        NativeArchitecture = nativeArchitecture;
        TargetArchitecture = targetArchitecture;
        Environments = environments;
    }

    public static ToolChainBuilder CreateFromEnvironment()
    {
        var binaryFinder = FileFinderBuilder.FromPathAndPathExt();
        var targetArchitecture = Architecture.DetectedNativeArchitecture.Value
                             ?? throw new PlatformNotSupportedException
                                 ("unknown architecture,call ToolChain construction function with architecture argument");
        var nativeArchitecture = targetArchitecture;

        var envs = Environment.GetEnvironmentVariables();

        var environments = new Dictionary<string, string>(envs.Keys.Count);

        foreach (var envKey in envs.Keys)
        {
            environments.Add(envKey.ToString()!, envs[envKey]!.ToString()!);
        }

        return new ToolChainBuilder(binaryFinder, nativeArchitecture, targetArchitecture, environments);
    }

    public static ToolChainBuilder CreateNativeArchitectureEmpty(
        FileFinderBuilder? finder = null,
        Dictionary<string, string>? environments = null)
    {
        return new ToolChainBuilder(
            finder ?? new FileFinderBuilder(),
            Architecture.DetectedNativeArchitecture.Value ?? throw new PlatformNotSupportedException("unknown architecture"),
            Architecture.DetectedNativeArchitecture.Value ?? throw new PlatformNotSupportedException("unknown architecture"),
            new());
    }

    public static ToolChainBuilder Create(
        Architecture nativeArchitecture,
        Architecture targetArchitecture,
        FileFinderBuilder? finder = null,
        Dictionary<string, string>? environments = null)
    {
        return new ToolChainBuilder(
            finder ?? new FileFinderBuilder(),
            nativeArchitecture,
            targetArchitecture,
            environments ?? []);
    }

    public ToolChain Build()
    {
        return new ToolChain(
            Tools.ToFrozenDictionary(),
            BinaryFinder.Build(),
            NativeArchitecture,
            TargetArchitecture,
            Environments.ToFrozenDictionary());
    }
}
