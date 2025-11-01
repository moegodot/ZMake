using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ZMake.Api;

public sealed class RootPathService(string rootSourcePath, string rootBuildPath)
{
    internal const string ZMakeProjectDirectoryName = "zmake";
    internal const string DotnetDirectoryName = "dotnet";

    public string RootSourcePath { get; } = rootSourcePath;
    public string RootBuildPath { get; } = rootBuildPath;
    public string RootBinaryObjectsPath { get; } = Path.Combine(rootBuildPath, "objects");
    public string RootCachePath { get; } = Path.Combine(rootBuildPath, "caches");
    public string RootZMakeProjectPath { get; } = Path.Combine(rootBuildPath, ZMakeProjectDirectoryName);
    public string RootDotnetPath { get; } = Path.Combine(rootBuildPath, DotnetDirectoryName);


    public PathService Create(string currentSourcePath)
    {
        currentSourcePath = Path.GetFullPath(currentSourcePath);

        var relative = Path.GetRelativePath(RootSourcePath, currentSourcePath);

        var binaryPath =
            Path.Combine(RootBinaryObjectsPath, relative);

        var cachePath = Path.Combine(RootCachePath, relative);

        return new PathService
        {
            CurrentSourceDir = currentSourcePath,
            CurrentBinaryObjectsDir = binaryPath,
            CurrentCacheDir = cachePath
        };
    }
}
