using System.Runtime.InteropServices;

namespace ZMake;

/// <summary>
/// This powerful program finder let you find everything you want.
/// </summary>
public class FileFinder
{
    public List<string> SearchPaths { get; init; } = [];

    public List<string> SearchSuffixes { get; init; } = [];

    /// <summary>
    /// Create file finder from environment variable "PATH".
    /// Also use environment variable "PATHEXT" in windows.
    /// </summary>
    public static FileFinder FromPathEnvironmentVariables()
    {
        var path = Environment.GetEnvironmentVariable("PATH");
        path ??= "";

        List<string> suffixes = [];
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            var pathext = Environment.GetEnvironmentVariable("PATHEXT");
            if (pathext != null)
            {
                suffixes.AddRange(pathext.Split(';'));
                suffixes.AddRange(pathext.Split(';').Select(s => s.ToLowerInvariant()));
            }
        }

        var splitter = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? ';' : ':';

        return new FileFinder
        {
            SearchPaths = path.Split(splitter).SkipWhile(string.IsNullOrWhiteSpace).ToList(),
            SearchSuffixes = suffixes
        };
    }

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
}
