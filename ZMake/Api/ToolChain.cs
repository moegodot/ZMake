using System.Collections.Frozen;

namespace ZMake.Api;

public sealed class ToolChain
{
    public FrozenDictionary<ToolType, List<ITool>> Tools { get; }
    public FileFinder BinaryFinder { get; }
    public Architecture NativeArchitecture { get; }
    public Architecture TargetArchitecture { get; }
    public FrozenDictionary<string, string> Environments { get; }

    public ITool? this[ToolType toolType] => Tools[toolType]?.FirstOrDefault();

    public ToolChain(
        FrozenDictionary<ToolType, List<ITool>> tools,
        FileFinder binaryFinder,
        Architecture nativeArchitecture,
        Architecture targetArchitecture,
        FrozenDictionary<string, string> environments)
    {
        Tools = tools;
        BinaryFinder = binaryFinder;
        NativeArchitecture = nativeArchitecture;
        TargetArchitecture = targetArchitecture;
        Environments = environments;
    }
}
