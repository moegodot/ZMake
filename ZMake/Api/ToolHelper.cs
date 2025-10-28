using Semver;

namespace ZMake.Api;

public static class ToolHelper
{
    public static IEnumerable<Task<ITool>> WithVersion(
        this IEnumerable<ITool> tools,
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

    public static IEnumerable<Task<ITool>> WithVersionGreaterThan(
        this IEnumerable<ITool> tools,
        SemVersion minimumVersion)
    {
        return tools.WithVersion(version => version.CompareSortOrderTo(minimumVersion) > 0);
    }

    public static IEnumerable<Task<ITool>> WithVersionLessenThan(
        this IEnumerable<ITool> tools,
        SemVersion maximumVersion)
    {
        return tools.WithVersion(version => version.CompareSortOrderTo(maximumVersion) < 0);
    }
}
