namespace ZMake.Api.BuiltIn;

public static class Architectures
{
    private static Architecture MakeBuiltinArchitecture(ArtifactName version, string name)
    {
        return Architecture.From(Name.Create(version, ["architecture", name]));
    }

    public static readonly Architecture X86 = MakeBuiltinArchitecture(Version.V1V0V0, "x86");
    public static readonly Architecture X64 = MakeBuiltinArchitecture(Version.V1V0V0, "x64");
    public static readonly Architecture Arm64 = MakeBuiltinArchitecture(Version.V1V0V0, "arm64");
}
