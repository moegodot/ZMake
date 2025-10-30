using System.Runtime.CompilerServices;

namespace ZMake.Api;

public sealed class PathService
{
    public required string CurrentSourceDir { get; init; }
    public required string CurrentBinaryObjectsDir { get; init; }
    public required string CurrentCacheDir { get; init; }
}
