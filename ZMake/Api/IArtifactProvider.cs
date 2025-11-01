namespace ZMake.Api;

public interface IArtifactsProvider
{
    void Initialize(InitializeContextBuilder builder);
    void Initialize(InitializeContext context);
}
