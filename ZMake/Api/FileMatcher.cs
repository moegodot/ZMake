using Microsoft.Extensions.FileSystemGlobbing;

namespace ZMake.Api;

public static class FileMatcher
{
    public static IEnumerable<string> Matches(string path,params string[] includes)
    {
        return Matches(path, includes, []);
    }

    public static IEnumerable<string> Matches(string path,string[] includes,string[] excludes)
    {
        Matcher matcher = new();
        matcher.AddIncludePatterns(includes);
        matcher.AddExcludePatterns(excludes);
        return matcher.GetResultsInFullPath(path);
    }
}
