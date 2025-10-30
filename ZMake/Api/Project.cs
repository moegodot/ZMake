namespace ZMake.Api;

public abstract class ArtifactProject(BuildContext buildContext)
{
    public BuildContext Context { get; } = buildContext;

    public abstract string GroupId { get; }
    public abstract string ArtifactId { get; }
    public abstract string Version { get; }

    private ArtifactName? _artifactName = null;

    public ArtifactName ArtifactName
    {
        get
        {
            if (_artifactName is not null) return _artifactName;
            if (GroupId is null || ArtifactId is null || Version is null)
            {
                throw new InvalidOperationException("GroupId, ArtifactId and Version must be set." +
                                                    "You may need to construct GroupId,ArtifactId and Version " +
                                                    "in the constructor and construct them **firstly** of the project.");
            }
            _artifactName = ArtifactName.Create(GroupId, ArtifactId, Version);
            return _artifactName;
        }
    }

    public abstract PathService Paths { get; }
}
