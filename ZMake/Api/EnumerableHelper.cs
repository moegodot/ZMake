namespace ZMake.Api;

public static class EnumerableHelper
{
    public static int GetEnumerableHashCode<T>(this IEnumerable<T> enumerable)
    {
        return enumerable.Aggregate(0, HashCode.Combine);
    }

    public static int GetDictionaryHashCode<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary)
    {
        return dictionary.Aggregate(0, (code,pair)=>
            HashCode.Combine(pair.Key?.GetHashCode() ?? 0,pair.Value?.GetHashCode() ?? 0, code));
    }

    public static bool DictionaryEquals<TKey, TValue>(
        this IReadOnlyDictionary<TKey, TValue> left,
        IReadOnlyDictionary<TKey, TValue> right)
    {
        var valueComparer = EqualityComparer<TValue>.Default;

        return  left.Count == right.Count &&
                left.Keys.All(key => right.ContainsKey(key) &&
                                     valueComparer.Equals(left[key], right[key]));
    }
}
