using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace ZMake.Api;

public sealed partial class Name : IEquatable<Name>, IParsable<Name>
{
    [GeneratedRegex("^[a-zA-Z_]+[a-zA-Z0-9_]*$")]
    private static partial Regex GeneratorNameRegex();

    public static readonly Regex NameRegex = GeneratorNameRegex();

    private Name(ArtifactName artifactName, string[] names)
    {
        ArtifactName = artifactName;
        Names = names;
    }

    public ArtifactName ArtifactName { get; init; }

    public IReadOnlyList<string> Names { get; init; }

    public bool Equals(Name? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return ArtifactName.Equals(other.ArtifactName) && Names.SequenceEqual(other.Names);
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? str,
        IFormatProvider? _,
        [NotNullWhen(true)] out Name? result)
    {
        result = null;

        if (str == null) return false;

        var part = str.Split('#');

        if (part.Length != 2) return false;

        if (ArtifactName.TryParse(part[0], _, out var artifact))
            return TryCreate(artifact, part[1].Split(':'), out result);

        return false;
    }

    public static Name Parse([NotNullWhen(true)] string? str, IFormatProvider? _)
    {
        ArgumentNullException.ThrowIfNull(str);

        if (!TryParse(str, _, out var result)) throw new ArgumentException("invalid name format");

        return result;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || (obj is Name other && Equals(other));
    }

    public override int GetHashCode()
    {
        return Names.Aggregate(ArtifactName.GetHashCode(), HashCode.Combine);
    }

    public override string ToString()
    {
        return $"{ArtifactName}#{string.Join(':', Names)}";
    }

    public static bool TryCreate(ArtifactName artifactName, IEnumerable<string> names,
        [NotNullWhen(true)] out Name? result)
    {
        result = null;
        var nameArray = names.ToArray();

        foreach (var name in nameArray)
            if (!NameRegex.IsMatch(name))
                return false;

        result = new Name(artifactName, nameArray);
        return true;
    }

    public static Name Create(ArtifactName artifactName, params IEnumerable<string> names)
    {
        if (!TryCreate(artifactName, names, out var result)) throw new ArgumentException("invalid names format");

        return result;
    }

}
