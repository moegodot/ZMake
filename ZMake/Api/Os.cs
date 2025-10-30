using System.Runtime.InteropServices;
using Vogen;
using ZMake.Api.BuiltIn;

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
                os= Oses.Windows;
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                os= Oses.Linux;
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                os= Oses.Macos;
            }

            LogMessage.DetectSystemInformation(
                LogMessage.Logger,
                nameof(Os),
                os?.ToString() ?? "unknown");

            return os;
        }, LazyThreadSafetyMode.None);
}
