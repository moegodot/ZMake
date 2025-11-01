using ZMake.Api;

namespace ZMake;

internal sealed class BuildPipeline
{
    public InitializeContextBuilder InitialBuilder { get; }
    public IArtifactsProvider Provider { get; }

    public BuildPipeline(
        InitializeContextBuilder initialBuilder,
        IArtifactsProvider provider)
    {
        InitialBuilder = initialBuilder;
        Provider = provider;
    }

    public BuildContext Build()
    {
        Provider.Initialize(InitialBuilder);
        var initializeContext = InitialBuilder.Build();
        Provider.Initialize(initializeContext);
        return initializeContext.Build();
    }
}
