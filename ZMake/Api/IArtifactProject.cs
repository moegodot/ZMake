using Semver;

namespace ZMake.Api;

public interface IArtifactProject : IArtifact
{
    static abstract string CurrentSourceDirectory { get; }

    static abstract ArtifactName GetArtifactName();
}
