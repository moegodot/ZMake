namespace ZMake.Api.BuiltIn;

public static class TargetTypes
{
    private static TargetType MakeBuiltinTargetName(ArtifactName version, string name)
    {
        return TargetType.From(Name.Create(version, $"target_set.{name}"));
    }

    public static readonly TargetType Initialize = MakeBuiltinTargetName(Version.V1V0V0, "initialize");
    public static readonly TargetType Build = MakeBuiltinTargetName(Version.V1V0V0, "build");
    public static readonly TargetType Clean = MakeBuiltinTargetName(Version.V1V0V0, "clean");
    public static readonly TargetType Test = MakeBuiltinTargetName(Version.V1V0V0, "test");
    public static readonly TargetType Package = MakeBuiltinTargetName(Version.V1V0V0, "package");
    public static readonly TargetType Install = MakeBuiltinTargetName(Version.V1V0V0, "install");
    public static readonly TargetType Deploy = MakeBuiltinTargetName(Version.V1V0V0, "deploy");
}
