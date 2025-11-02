using Vogen;

namespace ZMake.Api;

[ValueObject<Name>(conversions: Conversions.None)]
public partial class FileType
{
    public static class Builtin
    {
        private static FileType Make(ArtifactName version, string name)
        {
            return FileType.From(Name.Create(version, $"file_type.{name}"));
        }

        public static readonly FileType Source = Make(Version.V1V0V0,"source");
        public static readonly FileType Output = Make(Version.V1V0V0,"output");
    }
}
