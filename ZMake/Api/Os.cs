using System.Runtime.InteropServices;
using Vogen;
using ZMake.Api.BuiltIn;

namespace ZMake.Api;

[ValueObject<Name>]
public sealed partial class Os
{
    public static readonly Lazy<Os?> DetectedOs =
        new(() =>
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return Oses.Windows;
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return Oses.Linux;
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return Oses.Macos;
            }

            return null;
        }, LazyThreadSafetyMode.None);
}
