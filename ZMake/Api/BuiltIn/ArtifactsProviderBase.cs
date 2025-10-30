namespace ZMake.Api.BuiltIn;

public abstract class ArtifactsProviderBase : IArtifactsProvider
{
    public BuildContext? Context { get; private set; } = null;

    public void Bind(BuildContext context)
    {
        if (Context != null)
        {
            throw new InvalidOperationException("The artifacts provider has been bound to a context");
        }
        Context = context;
    }

    protected abstract void AfterBind();

    public abstract IEnumerable<Artifact> Get();
}
