using System.Net;
using Tenray.ZoneTree.Serializers;

namespace ZMake.Api;

public class UInt128Serializer:ISerializer<UInt128>
{
    public UInt128 Deserialize(Memory<byte> bytes)
    {
        return BitConverter.ToUInt128(bytes.Span);
    }

    public Memory<byte> Serialize(in UInt128 entry)
    {
        return BitConverter.GetBytes(entry);
    }
}
