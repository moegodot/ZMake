using Semver;

namespace ZMake.Api;

public static class ToolHelper
{
    public static IEnumerable<Task<ITool<T>>> WithVersion<T>(
        this IEnumerable<ITool<T>> tools,
        Func<SemVersion,bool> requirements)
    {
        return tools
            .Select(async tool =>
            {
                await tool.GetVersionAsync();
                return tool;
            })
            .SkipWhile(tool => tool.IsCompletedSuccessfully && tool.Result.GetVersion() != null)
            .TakeWhile(tool => requirements(tool.Result.GetVersion()!));
    }

    public static IEnumerable<Task<ITool<T>>> WithVersionGreaterThan<T>(
        this IEnumerable<ITool<T>> tools,
        SemVersion minimumVersion)
    {
        return tools.WithVersion(version => version.CompareSortOrderTo(minimumVersion) > 0);
    }

    public static IEnumerable<Task<ITool<T>>> WithVersionLessenThan<T>(
        this IEnumerable<ITool<T>> tools,
        SemVersion maximumVersion)
    {
        return tools.WithVersion(version => version.CompareSortOrderTo(maximumVersion) < 0);
    }
}
