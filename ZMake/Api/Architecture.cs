using System.Diagnostics;
using System.Runtime.InteropServices;
using Vogen;
using ZMake.Api.BuiltIn;

namespace ZMake.Api;

[ValueObject<Name>]
public sealed partial class Architecture
{
    public static readonly Lazy<Architecture?> DetectedNativeArchitecture =
        new(() =>
        {
            return RuntimeInformation.ProcessArchitecture switch
            {
                System.Runtime.InteropServices.Architecture.X86 => Architectures.X86,
                System.Runtime.InteropServices.Architecture.X64 => Architectures.X64,
                System.Runtime.InteropServices.Architecture.Arm64 => Architectures.Arm64,
                _ => null
            };
        }, LazyThreadSafetyMode.None);
}
