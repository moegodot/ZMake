namespace ZMake.Api;

public interface IArtifactsProvider
{
    BuildContext? Context { get; }
    void Bind(BuildContext context);
    IEnumerable<Artifact> Get();
}
