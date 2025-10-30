using System.Diagnostics;
using System.Runtime.InteropServices;
using Vogen;
using ZMake.Api.BuiltIn;

namespace ZMake.Api;

[ValueObject<Name>(conversions: Conversions.None)]
public sealed partial class Architecture
{
    public static readonly Lazy<Architecture?> DetectedNativeArchitecture =
        new(() =>
        {
            var arch = RuntimeInformation.ProcessArchitecture switch
            {
                System.Runtime.InteropServices.Architecture.X86 => Architectures.X86,
                System.Runtime.InteropServices.Architecture.X64 => Architectures.X64,
                System.Runtime.InteropServices.Architecture.Arm64 => Architectures.Arm64,
                _ => null
            };

            LogMessage.DetectSystemInformation(
                LogMessage.Logger,
                nameof(Architecture),
                arch?.ToString() ?? "unknown");

            return arch;
        }, LazyThreadSafetyMode.None);
}
