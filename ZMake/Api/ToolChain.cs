namespace ZMake.Api;

public class ToolChain
{
    public Dictionary<ToolType, ITool> Tools { get; } = [];

    public FileFinder Finder { get; }

    public Architecture NativeArchitecture { get; }

    public Architecture TargetArchitecture { get; }

    public ToolChain()
    {
        Finder = FileFinder.FromPathEnvironmentVariables();
        TargetArchitecture = Architecture.DetectedNativeArchitecture.Value
                             ?? throw new PlatformNotSupportedException
                                 ("unknown architecture,call ToolChain construction function with architecture argument");
        NativeArchitecture = TargetArchitecture;
    }

    public ToolChain(FileFinder finder,Architecture nativeArchitecture,Architecture targetArchitecture)
    {
        Finder = finder;
        NativeArchitecture = nativeArchitecture;
        TargetArchitecture = targetArchitecture;
    }
}
