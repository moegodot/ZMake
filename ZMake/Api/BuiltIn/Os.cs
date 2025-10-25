namespace ZMake.Api.BuiltIn;

public static class Os
{
    private static Architecture MakeBuiltinOs(ArtifactName version, string name)
    {
        return Architecture.From(Name.Create(version, ["os", name]));
    }

    public static readonly Architecture Windows = MakeBuiltinOs(Version.V1V0V0, "windows");
    public static readonly Architecture Linux = MakeBuiltinOs(Version.V1V0V0, "linux");
    public static readonly Architecture Macos = MakeBuiltinOs(Version.V1V0V0, "macos");
}
