using Semver;

namespace ZMake.Api;

public static class Version
{
    internal static readonly string GroupId = "moe.kawayi";
    internal static readonly string ArtifactId = "zmake";

    public static readonly ArtifactName V1V0V0 =
        ArtifactName.Create(
            GroupId,
            ArtifactId,
            new SemVersion(1, 0, 0));

    public static readonly ArtifactName Latest = V1V0V0;
}
