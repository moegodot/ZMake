using System.Collections.Immutable;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace ZMake.Api;

/// <summary>
/// This powerful program finder let you find everything you want.
/// </summary>
[PublicAPI]
public sealed class FileFinder
{
    public ImmutableArray<string> SearchPaths { get; init; } = [];

    public ImmutableArray<string> SearchSuffixes { get; init; } = [];

    public IEnumerable<string> Search(string targetName)
    {
        foreach (var path in SearchPaths)
        {
            var withOutSuffix = Path.Combine(path, targetName);
            if (File.Exists(withOutSuffix))
            {
                yield return withOutSuffix;
            }

            foreach (var suffix in SearchSuffixes)
            {
                var withSuffix = Path.Combine(path, suffix);
                if (File.Exists(withSuffix))
                {
                    yield return withSuffix;
                }
            }
        }
    }

    public FileFinder(ImmutableArray<string> searchPaths, ImmutableArray<string> searchSuffixes)
    {
        SearchPaths = searchPaths;
        SearchSuffixes = searchSuffixes;
    }
}
