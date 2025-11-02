namespace ZMake.Api;

public static class BuildConstantExtension
{

    public static BuildConstantBuilder ToBuildConstantBuilderAsSource(
        this IEnumerable<string> source,
        IContext context)
    {
        return new BuildConstantBuilder(context)
            .WithSources(source);
    }
}
