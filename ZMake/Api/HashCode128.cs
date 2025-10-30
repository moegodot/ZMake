using System.Runtime.InteropServices;
using Standart.Hash.xxHash;

namespace ZMake.Api;

public static class HashCode128
{
    public static UInt128 ToUint128(this uint128 input)
    {
        return new UInt128(input.high64, input.low64);
    }

    public static UInt128 Combine(UInt128 one, UInt128 two)
    {
        unsafe
        {
            var pointer = stackalloc byte[32];

            Marshal.StructureToPtr(one, (nint)pointer, false);
            Marshal.StructureToPtr(two, (nint)(pointer + 16), false);

            var uint128 = ToUint128(xxHash128.ComputeHash(new Span<byte>(pointer, 32), 32));
            return uint128;
        }
    }

    public static UInt128 Combine(UInt128 t1, UInt128 t2, UInt128 t3)
    {
        unsafe
        {
            var pointer = stackalloc byte[48];

            Marshal.StructureToPtr(t1, (nint)pointer, false);
            Marshal.StructureToPtr(t2, (nint)(pointer + 16), false);
            Marshal.StructureToPtr(t3, (nint)(pointer + 32), false);

            var uint128 = ToUint128(xxHash128.ComputeHash(new Span<byte>(pointer, 48), 48));
            return uint128;
        }
    }

    public static UInt128 Combine(UInt128 t1, UInt128 t2, UInt128 t3, UInt128 t4)
    {
        unsafe
        {
            var pointer = stackalloc byte[64];

            Marshal.StructureToPtr(t1, (nint)pointer, false);
            Marshal.StructureToPtr(t2, (nint)(pointer + 16), false);
            Marshal.StructureToPtr(t3, (nint)(pointer + 32), false);
            Marshal.StructureToPtr(t4, (nint)(pointer + 48), false);

            var uint128 = ToUint128(xxHash128.ComputeHash(new Span<byte>(pointer, 64), 64));
            return uint128;
        }
    }

    public static UInt128 Combine(UInt128 t1, UInt128 t2, UInt128 t3, UInt128 t4, UInt128 t5)
    {
        unsafe
        {
            var pointer = stackalloc byte[80];

            Marshal.StructureToPtr(t1, (nint)pointer, false);
            Marshal.StructureToPtr(t2, (nint)(pointer + 16), false);
            Marshal.StructureToPtr(t3, (nint)(pointer + 32), false);
            Marshal.StructureToPtr(t4, (nint)(pointer + 48), false);
            Marshal.StructureToPtr(t5, (nint)(pointer + 64), false);

            var uint128 = ToUint128(xxHash128.ComputeHash(new Span<byte>(pointer, 80), 80));
            return uint128;
        }
    }


    public static UInt128 Combine(UInt128 t1, UInt128 t2, UInt128 t3, UInt128 t4, UInt128 t5, UInt128 t6)
    {
        unsafe
        {
            var pointer = stackalloc byte[96];

            Marshal.StructureToPtr(t1, (nint)pointer, false);
            Marshal.StructureToPtr(t2, (nint)(pointer + 16), false);
            Marshal.StructureToPtr(t3, (nint)(pointer + 32), false);
            Marshal.StructureToPtr(t4, (nint)(pointer + 48), false);
            Marshal.StructureToPtr(t5, (nint)(pointer + 64), false);
            Marshal.StructureToPtr(t6, (nint)(pointer + 80), false);

            var uint128 = ToUint128(xxHash128.ComputeHash(new Span<byte>(pointer, 96), 96));
            return uint128;
        }
    }

    public static UInt128 Combine(UInt128 t1, UInt128 t2, UInt128 t3, UInt128 t4, UInt128 t5, UInt128 t6, UInt128 t7)
    {
        unsafe
        {
            var pointer = stackalloc byte[112];

            Marshal.StructureToPtr(t1, (nint)pointer, false);
            Marshal.StructureToPtr(t2, (nint)(pointer + 16), false);
            Marshal.StructureToPtr(t3, (nint)(pointer + 32), false);
            Marshal.StructureToPtr(t4, (nint)(pointer + 48), false);
            Marshal.StructureToPtr(t5, (nint)(pointer + 64), false);
            Marshal.StructureToPtr(t6, (nint)(pointer + 80), false);
            Marshal.StructureToPtr(t7, (nint)(pointer + 96), false);

            var uint128 = ToUint128(xxHash128.ComputeHash(new Span<byte>(pointer, 112), 112));
            return uint128;
        }
    }
    public static UInt128 Combine(
        UInt128 t1,
        UInt128 t2,
        UInt128 t3,
        UInt128 t4,
        UInt128 t5,
        UInt128 t6,
        UInt128 t7,
        UInt128 t8)
    {
        unsafe
        {
            var pointer = stackalloc byte[128];

            Marshal.StructureToPtr(t1, (nint)pointer, false);
            Marshal.StructureToPtr(t2, (nint)(pointer + 16), false);
            Marshal.StructureToPtr(t3, (nint)(pointer + 32), false);
            Marshal.StructureToPtr(t4, (nint)(pointer + 48), false);
            Marshal.StructureToPtr(t5, (nint)(pointer + 64), false);
            Marshal.StructureToPtr(t6, (nint)(pointer + 80), false);
            Marshal.StructureToPtr(t7, (nint)(pointer + 96), false);
            Marshal.StructureToPtr(t8, (nint)(pointer + 112), false);

            var uint128 = ToUint128(xxHash128.ComputeHash(new Span<byte>(pointer, 128), 128));
            return uint128;
        }
    }

    public static UInt128 Combine<T1, T2>(T1 t1, T2 t2)
        where T1 : IGetHashCode128
        where T2 : IGetHashCode128
    {
        return Combine(t1.GetHashCode128(), t2.GetHashCode128());
    }

    public static UInt128 Combine<T1, T2, T3>(T1 t1, T2 t2, T3 t3)
        where T1 : IGetHashCode128
        where T2 : IGetHashCode128
        where T3 : IGetHashCode128
    {
        return Combine(t1.GetHashCode128(), t2.GetHashCode128(), t3.GetHashCode128());
    }

    public static UInt128 Combine<T1, T2, T3, T4>(T1 t1, T2 t2, T3 t3, T4 t4)
        where T1 : IGetHashCode128
        where T2 : IGetHashCode128
        where T3 : IGetHashCode128
        where T4 : IGetHashCode128
    {
        return Combine(t1.GetHashCode128(), t2.GetHashCode128(), t3.GetHashCode128(), t4.GetHashCode128());
    }

    public static UInt128 Combine<T1, T2, T3, T4, T5>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
        where T1 : IGetHashCode128
        where T2 : IGetHashCode128
        where T3 : IGetHashCode128
        where T4 : IGetHashCode128
        where T5 : IGetHashCode128
    {
        return Combine(t1.GetHashCode128(),
            t2.GetHashCode128(),
            t3.GetHashCode128(),
            t4.GetHashCode128(),
            t5.GetHashCode128());
    }

    public static UInt128 Combine<T1, T2, T3, T4, T5, T6>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
        where T1 : IGetHashCode128
        where T2 : IGetHashCode128
        where T3 : IGetHashCode128
        where T4 : IGetHashCode128
        where T5 : IGetHashCode128
        where T6 : IGetHashCode128
    {
        return Combine(t1.GetHashCode128(), t2.GetHashCode128(), t3.GetHashCode128(), t4.GetHashCode128(), t5.GetHashCode128(), t6.GetHashCode128());
    }

    public static UInt128 Combine<T1, T2, T3, T4, T5, T6, T7>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
        where T1 : IGetHashCode128
        where T2 : IGetHashCode128
        where T3 : IGetHashCode128
        where T4 : IGetHashCode128
        where T5 : IGetHashCode128
        where T6 : IGetHashCode128
        where T7 : IGetHashCode128
    {
        return Combine(t1.GetHashCode128(), t2.GetHashCode128(),
            t3.GetHashCode128(), t4.GetHashCode128(),
            t5.GetHashCode128(), t6.GetHashCode128(),
            t7.GetHashCode128());
    }

    public static UInt128 Combine<T1, T2, T3, T4, T5, T6, T7, T8>(
        T1 t1,
        T2 t2,
        T3 t3,
        T4 t4,
        T5 t5,
        T6 t6,
        T7 t7,
        T8 t8)
        where T1 : IGetHashCode128
        where T2 : IGetHashCode128
        where T3 : IGetHashCode128
        where T4 : IGetHashCode128
        where T5 : IGetHashCode128
        where T6 : IGetHashCode128
        where T7 : IGetHashCode128
        where T8 : IGetHashCode128
    {
        return Combine(
            t1.GetHashCode128(),
            t2.GetHashCode128(),
            t3.GetHashCode128(),
            t4.GetHashCode128(),
            t5.GetHashCode128(),
            t6.GetHashCode128(),
            t7.GetHashCode128(),
            t8.GetHashCode128());
    }

    public static UInt128 Get(string str)
    {
        return ToUint128(xxHash128.ComputeHash(str));
    }

    public static UInt128 Get<T>(T num)
    where T : struct
    {
        unsafe
        {
            var size = Marshal.SizeOf<T>(num);
            var pointer = stackalloc byte[size];
            Marshal.StructureToPtr<T>(num, (nint)pointer, false);
            var span = new Span<byte>(pointer, size);
            var uint128 = ToUint128(xxHash128.ComputeHash(span, size));
            Marshal.DestroyStructure<T>((nint)pointer);
            return uint128;
        }
    }

    public static UInt128 GetEnumerableHashCode128<T>(this IEnumerable<T> enumerable)
    where T : IGetHashCode128
    {
        return enumerable.Aggregate(UInt128.Zero, (num, obj) =>
            Combine(num, obj.GetHashCode128()));
    }

    public static UInt128 GetEnumerableHashCode128(this IEnumerable<string> enumerable)
    {
        return enumerable.Aggregate(UInt128.Zero, (num, obj) =>
            Combine(num, Get(obj)));
    }

    public static UInt128 GetDictionaryHashCode128<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary)
    where TKey : IGetHashCode128 where TValue : IGetHashCode128
    {
        return dictionary.Aggregate(UInt128.Zero, (code, pair) =>
            Combine(pair.Key.GetHashCode128(), pair.Value.GetHashCode128(), code));
    }

    public static UInt128 GetDictionaryHashCode128(this IReadOnlyDictionary<string, string> dictionary)
    {
        return dictionary.Aggregate(UInt128.Zero, (code, pair) =>
            Combine(Get(pair.Key), Get(pair.Value), code));
    }
}
