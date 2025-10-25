using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Semver;

namespace ZMake.Api;

/// <summary>
///     Artifact coordinates are most often represented as groupId:artifactId:version.
///     It should be constant.
/// </summary>
public sealed class ArtifactName : IEquatable<ArtifactName>, IParsable<ArtifactName>
{
    public static readonly Regex GroupIdRegex = new("^[a-zA-Z_]+[a-zA-Z0-9_]*(\\.[a-zA-Z_]+[a-zA-Z0-9_]*)+$");
    public static readonly Regex ArtifactIdRegex = new("^[a-zA-Z_]+[a-zA-Z0-9_]*$");

    private ArtifactName(string groupId, string artifactId, SemVersion version)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(groupId);
        ArgumentException.ThrowIfNullOrWhiteSpace(artifactId);
        ArgumentNullException.ThrowIfNull(version);
        GroupId = groupId;
        ArtifactId = artifactId;
        Version = version;
    }

    public string GroupId { get; init; }
    public string ArtifactId { get; init; }

    /// <summary>
    ///     Semver 2.0
    /// </summary>
    public SemVersion Version { get; init; }

    public bool Equals(ArtifactName? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return GroupId.Equals(other.GroupId) && ArtifactId.Equals(other.ArtifactId) && Version.Equals(other.Version);
    }

    public static ArtifactName Parse(string text, IFormatProvider? provider)
    {
        return !TryParse(text, out var result, out var reason) ? throw new FormatException(reason) : result;
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out ArtifactName result)
    {
        if (s is not null) return TryParse(s, out result, out _);
        result = null;
        return false;
    }

    public static ArtifactName CreateWithNewVersion(ArtifactName oldArtifact, SemVersion newVersion)
    {
        return new ArtifactName(oldArtifact.GroupId, oldArtifact.ArtifactId, newVersion);
    }

    public static bool TryParse(
        string text,
        [NotNullWhen(true)] out ArtifactName? result,
        [NotNullWhen(false)] out string? reason)
    {
        result = null;
        reason = null;

        var splitted = text.Split(':');

        if (splitted.Length != 3)
        {
            reason = "can not found two `:` in the string";
            return false;
        }

        return TryCreate(splitted[0], splitted[1],
            splitted[2], out result, out reason);
    }

    public static bool TryCreate(
        string groupId,
        string artifactId,
        string version,
        [NotNullWhen(true)] out ArtifactName? result,
        [NotNullWhen(false)] out string? reason)
    {
        result = null;
        reason = null;

        if (!GroupIdRegex.IsMatch(groupId))
        {
            reason = "invalid groupId format";
            return false;
        }

        if (!ArtifactIdRegex.IsMatch(artifactId))
        {
            reason = "invalid artifactId format";
            return false;
        }

        if (!SemVersion.TryParse(version, SemVersionStyles.Strict, out var semver))
        {
            reason = "invalid version(semver 2.0) format";
            return false;
        }

        result = new ArtifactName(groupId, artifactId, semver);
        return true;
    }

    public static ArtifactName Create(string groupId,
        string artifactId,
        string version)
    {
        return !TryCreate(groupId, artifactId, version,
            out var result, out var reason)
            ? throw new FormatException(reason)
            : result;
    }

    public static ArtifactName Create(string groupId,
        string artifactId,
        SemVersion version)
    {
        return !TryCreate(groupId, artifactId, version.ToString(),
            out var result, out var reason)
            ? throw new FormatException(reason)
            : result;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || (obj is ArtifactName other && Equals(other));
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
