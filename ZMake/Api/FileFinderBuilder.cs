using System.Collections.Immutable;
using System.Runtime.InteropServices;

namespace ZMake.Api;

public class FileFinderBuilder
{
    public List<string> SearchPaths { get; init; } = [];

    public List<string> SearchSuffixes { get; init; } = [];

    /// <summary>
    /// Create file finder from environment variable "PATH".
    /// Also use environment variable "PATHEXT" in windows.
    /// </summary>
    public static FileFinderBuilder FromPathAndPathExt()
    {
        var path = Environment.GetEnvironmentVariable("PATH");

        var splitter = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? ';' : ':';

        path ??= "";

        var finder = FromPathExt();

        finder.SearchPaths.AddRange(path.Split(splitter).SkipWhile(string.IsNullOrWhiteSpace));

        return finder;
    }

    public static FileFinderBuilder FromPathExt()
    {
        List<string> suffixes = [];
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            var pathExts = Environment.GetEnvironmentVariable("PATHEXT");
            if (pathExts != null)
            {
                suffixes.AddRange(pathExts.Split(';')); // .EXE
                suffixes.AddRange(pathExts.Split(';').Select(s => s.ToLowerInvariant())); // .exe in case modified ntfs case sensitivity
            }
        }

        return new FileFinderBuilder
        {
            SearchSuffixes = suffixes,
            SearchPaths = []
        };
    }

    public FileFinder Build()
    {
        return new FileFinder(
            SearchPaths.ToImmutableArray(),
            SearchSuffixes.ToImmutableArray());
    }
}
