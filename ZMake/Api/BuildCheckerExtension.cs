namespace ZMake.Api;

public static class BuildCheckerExtension
{
    public static TaskBuilder WithChecker(
        this TaskBuilder builder,
        BuildConstant buildConstant,
        IBuildChecker checker)
    {
        var func = builder.Func;
        builder.Func = async () =>
        {
            if (!await checker.CheckChanged(buildConstant))
            {
                await func();
            }
        };
        return builder;
    }
}
