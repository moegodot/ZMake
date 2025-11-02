using System.Runtime.InteropServices;
using Vogen;

namespace ZMake.Api;

[ValueObject<Name>(conversions: Conversions.None)]
public sealed partial class Os
{
    public static readonly Lazy<Os?> DetectedOs =
        new(() =>
        {
            Os? os = null;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                os= Builtin.Windows;
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                os= Builtin.Linux;
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                os= Builtin.Macos;
            }

            LogMessage.DetectSystemInformation(
                LogMessage.Logger,
                nameof(Os),
                os?.ToString() ?? "unknown");

            return os;
        }, LazyThreadSafetyMode.None);

    public static class Builtin
    {
        private static Os Make(ArtifactName version, string name)
        {
            return From(Name.Create(version, $"os.{name}"));
        }

        public static readonly Os Windows = Make(Version.V1V0V0, "windows");
        public static readonly Os Linux = Make(Version.V1V0V0, "linux");
        public static readonly Os Macos = Make(Version.V1V0V0, "macos");
    }
}
