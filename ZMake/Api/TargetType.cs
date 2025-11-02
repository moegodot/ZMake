using Vogen;

namespace ZMake.Api;

[ValueObject<Name>(conversions: Conversions.None)]
public sealed partial class TargetType
{
    public static class Builtin
    {
        private static TargetType Make(ArtifactName version, string name)
        {
            return TargetType.From(Name.Create(version, $"target_type.{name}"));
        }

        public static readonly TargetType Initialize = Make(Version.V1V0V0, "initialize");
        public static readonly TargetType Build = Make(Version.V1V0V0, "build");
        public static readonly TargetType Clean = Make(Version.V1V0V0, "clean");
        public static readonly TargetType Test = Make(Version.V1V0V0, "test");
        public static readonly TargetType Package = Make(Version.V1V0V0, "package");
        public static readonly TargetType Install = Make(Version.V1V0V0, "install");
        public static readonly TargetType Deploy = Make(Version.V1V0V0, "deploy");
    }
}
