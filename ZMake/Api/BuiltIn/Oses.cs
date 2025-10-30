namespace ZMake.Api.BuiltIn;

public static class Oses
{
    private static Os MakeBuiltinOs(ArtifactName version, string name)
    {
        return Os.From(Name.Create(version, $"os.{name}"));
    }

    public static readonly Os Windows = MakeBuiltinOs(Version.V1V0V0, "windows");
    public static readonly Os Linux = MakeBuiltinOs(Version.V1V0V0, "linux");
    public static readonly Os Macos = MakeBuiltinOs(Version.V1V0V0, "macos");
    public static readonly Os Android = MakeBuiltinOs(Version.V1V0V0, "android");
    public static readonly Os Ios = MakeBuiltinOs(Version.V1V0V0, "ios");
}
