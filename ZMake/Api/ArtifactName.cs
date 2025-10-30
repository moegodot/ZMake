using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Semver;

namespace ZMake.Api;

/// <summary>
///     Artifact coordinates are most often represented as groupId:artifactId:version.
///     It should be constant.
/// </summary>
public sealed class ArtifactName :
    IEquatable<ArtifactName>,
    ISpanParsable<ArtifactName>
{
    [PublicAPI] public static readonly Regex GroupIdRegex = NameRule.MustHaveMultiplePartsRegex();

    [PublicAPI] public static readonly Regex ArtifactIdRegex = NameRule.MaybeSinglePartRegex();

    private static FormatException InvalidArtifactNameFormatException(string name)
    {
        return new FormatException($"invalid artifact name format: {name}");
    }

    private ArtifactName(string groupId, string artifactId, SemVersion version)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(groupId);
        ArgumentException.ThrowIfNullOrWhiteSpace(artifactId);
        ArgumentNullException.ThrowIfNull(version);
        GroupId = groupId;
        ArtifactId = artifactId;
        Version = version;
    }

    [PublicAPI]
    public string GroupId { get; }

    [PublicAPI]
    public string ArtifactId { get; }

    /// <summary>
    ///     Semver 2.0
    /// </summary>
    [PublicAPI]
    public SemVersion Version { get; }

    public bool Equals(ArtifactName? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return GroupId.Equals(other.GroupId)
            && ArtifactId.Equals(other.ArtifactId)
            && Version.Equals(other.Version);
    }

    public static ArtifactName Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (!TryParse(s, provider, out var result))
            throw InvalidArtifactNameFormatException(s.ToString());

        return result;
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
    IFormatProvider? provider,
    [MaybeNullWhen(false)] out ArtifactName result)
    {
        unsafe
        {
            Span<Range> parts = stackalloc Range[3];

            var splitCount = s.Split(parts, ':');

            if (splitCount != 3)
            {
                result = null;
                return false;
            }

            return TryCreate(
                s[parts[0]],
                s[parts[1]],
                s[parts[2]],
                out result);
        }
    }

    public static ArtifactName Parse(string text, IFormatProvider? provider)
    {
        return Parse((ReadOnlySpan<char>)text, provider);
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out ArtifactName result)
    {
        return TryParse((ReadOnlySpan<char>)s, provider, out result);
    }

    public static ArtifactName CreateWithNewVersion(ArtifactName oldArtifact, SemVersion newVersion)
    {
        return new ArtifactName(oldArtifact.GroupId, oldArtifact.ArtifactId, newVersion);
    }

    public static ArtifactName CreateWithNewArtifactId(ArtifactName oldArtifact, string newArtifactId)
    {
        return new ArtifactName(oldArtifact.GroupId, newArtifactId, oldArtifact.Version);
    }

    public static ArtifactName CreateWithNewGroupId(ArtifactName oldArtifact, string newGroupId)
    {
        return new ArtifactName(newGroupId, oldArtifact.ArtifactId, oldArtifact.Version);
    }

    public static bool TryCreate(
        ReadOnlySpan<char> groupId,
        ReadOnlySpan<char> artifactId,
        SemVersion version,
        [NotNullWhen(true)] out ArtifactName? result)
    {
        result = null;

        if (!GroupIdRegex.IsMatch(groupId))
        {
            return false;
        }

        if (!ArtifactIdRegex.IsMatch(artifactId))
        {
            return false;
        }

        result = new ArtifactName(groupId.ToString(), artifactId.ToString(), version);

        return true;
    }

    [PublicAPI]
    public static bool TryCreate(
        ReadOnlySpan<char> groupId,
        ReadOnlySpan<char> artifactId,
        ReadOnlySpan<char> version,
        [NotNullWhen(true)] out ArtifactName? result)
    {
        var versionString = version.ToString();
        if (!SemVersion.TryParse(versionString, SemVersionStyles.Strict, out var semver))
        {
            result = null;
            return false;
        }

        return TryCreate(groupId, artifactId, semver, out result);
    }

    public static ArtifactName Create(
        ReadOnlySpan<char> groupId,
        ReadOnlySpan<char> artifactId,
        ReadOnlySpan<char> version)
    {
        if (!TryCreate(groupId, artifactId, version, out var result))
            throw InvalidArtifactNameFormatException($"{groupId}:{artifactId}:{version}");

        return result;
    }

    public static ArtifactName Create(
        ReadOnlySpan<char> groupId,
        ReadOnlySpan<char> artifactId,
        SemVersion version)
    {
        if (!TryCreate(groupId, artifactId, version, out var result))
            throw InvalidArtifactNameFormatException($"{groupId}:{artifactId}:{version}");

        return result;
    }

    bool IEquatable<ArtifactName>.Equals(ArtifactName? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return GroupId.Equals(other.GroupId)
            && ArtifactId.Equals(other.ArtifactId)
            && Version.Equals(other.Version);
    }

    public override bool Equals(object? obj)
    {
        return obj is not null &&
        (ReferenceEquals(this, obj)
        || (obj is ArtifactName other && ((IEquatable<ArtifactName>)this).Equals(other)));
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(GroupId, ArtifactId, Version);
    }

    public override string ToString()
    {
        return $"{GroupId}:{ArtifactId}:{Version}";
    }
}
