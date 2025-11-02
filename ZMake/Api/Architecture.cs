using System.Diagnostics;
using System.Runtime.InteropServices;
using Vogen;

namespace ZMake.Api;

[ValueObject<Name>(conversions: Conversions.None)]
public sealed partial class Architecture
{
    public static readonly Lazy<Architecture?> DetectedNativeArchitecture =
        new(() =>
        {
            var arch = RuntimeInformation.ProcessArchitecture switch
            {
                System.Runtime.InteropServices.Architecture.X86 => Builtin.X86,
                System.Runtime.InteropServices.Architecture.X64 => Builtin.X64,
                System.Runtime.InteropServices.Architecture.Arm64 => Builtin.Arm64,
                _ => null
            };

            LogMessage.DetectSystemInformation(
                LogMessage.Logger,
                nameof(Architecture),
                arch?.ToString() ?? "unknown");

            return arch;
        }, LazyThreadSafetyMode.None);

    public static class Builtin
    {
        private static Architecture Make(ArtifactName version, string name)
        {
            return Architecture.From(Name.Create(version, $"architecture.{name}"));
        }

        public static readonly Architecture X86 = Make(Version.V1V0V0, "x86");
        public static readonly Architecture X64 = Make(Version.V1V0V0, "x64");
        public static readonly Architecture Arm64 = Make(Version.V1V0V0, "arm64");
    }
}
