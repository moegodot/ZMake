namespace ZMake.Api;

public interface IArtifact
{
    ArtifactName Name { get; }

    void PreInitialize(InitializeContext context)
    {

    }

    void Initialize(InitializeContext context)
    {

    }

    IEnumerable<ITargetSource> GetTargets(InitializeContext context)
    {
        return [];
    }

    void ConfigureTargetSet(
        InitializeContext context,
        ref TargetSetBuilder targetSetBuilder)
    {

    }

    void EndInitialize(Artifact initializeResult)
    {

    }
}
