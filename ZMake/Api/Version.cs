using Semver;

namespace ZMake.Api;

public static class Version
{
    public static readonly ArtifactName V1V0V0 =
        ArtifactName.Create(
            "moe.kawayi",
            "zmake",
            new SemVersion(1, 0, 0));

    public static readonly ArtifactName Latest = V1V0V0;
}
