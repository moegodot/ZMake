using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.RegularExpressions;

namespace ZMake.Api;

public sealed class Name :
    IEquatable<Name>,
    ISpanParsable<Name>
{
    public static readonly Regex NameRegex = NameRule.MaybeSinglePartRegex();

    private static FormatException InvalidNameFormatException(ReadOnlySpan<char> name)
    {
        return new FormatException($"invalid name format: {name}");
    }

    private Name(ArtifactName artifactName, string itemName)
    {
        ArtifactName = artifactName;
        ItemName = itemName;
    }

    public ArtifactName ArtifactName { get; }

    public string ItemName { get; }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || (obj is Name other && Equals(other));
    }

    public bool Equals(Name? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return ArtifactName.Equals(other.ArtifactName) && ItemName.Equals(other.ItemName);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(ArtifactName, ItemName);
    }

    public override string ToString()
    {
        return $"{ArtifactName}#{ItemName}";
    }

    public static Name Create(ArtifactName artifactName, ReadOnlySpan<char> name)
    {
        if (!TryCreate(artifactName, name, out var result))
            throw InvalidNameFormatException(name);

        return result;
    }

    public static bool TryCreate(
    ArtifactName artifactName,
    ReadOnlySpan<char> itemName,
    [NotNullWhen(true)] out Name? result)
    {
        result = null;

        if (!NameRegex.IsMatch(itemName))
            return false;

        result = new Name(artifactName, itemName.ToString());

        return true;
    }

    public static Name Parse(
        [NotNullWhen(true)] string? str,
        IFormatProvider? provider)
    {
        if (TryParse(str, provider, out var result))
        {
            return result;
        }

        throw InvalidNameFormatException(str);
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? str,
        IFormatProvider? provider,
        [NotNullWhen(true)] out Name? result)
    {
        return TryParse((ReadOnlySpan<char>)str, provider, out result);
    }

    public static Name Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        throw InvalidNameFormatException(s);
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out Name result)
    {
        result = null;
        unsafe
        {
            Span<Range> parts = stackalloc Range[2];

            var splitCount = s.Split(parts, '#');

            if (splitCount != 2)
            {
                return false;
            }

            if (!ArtifactName.TryParse(s[parts[0]], provider, out var artifactName))
            {
                return false;
            }

            return TryCreate(artifactName, s[parts[1]], out result);
        }
    }

    public Name Append(ReadOnlySpan<char> itemName)
    {
        if (!NameRegex.IsMatch(itemName)) { throw InvalidNameFormatException(itemName); }
        return new Name(ArtifactName, $"{ItemName}.{itemName}");
    }
}
