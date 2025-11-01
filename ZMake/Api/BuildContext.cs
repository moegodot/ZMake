using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;

namespace ZMake.Api;

public sealed class BuildContext : IContext
{
    public long Id { get; } =
        ((long)RandomNumberGenerator.GetInt32(int.MinValue, int.MaxValue) << 32) |
        (uint)RandomNumberGenerator.GetInt32(int.MinValue, int.MaxValue);

    public required TaskEngine TaskEngine { get; init; }

    private readonly CancellationTokenSource _tokenSource = new();

    public CancellationToken CancellationToken => _tokenSource.Token;

    public required FrozenDictionary<ArtifactName, Artifact> Artifacts { get; init; }

    public required RootPathService PathService { get; init; }

    public required ToolChain ToolChain { get; init; }

    public Task BuildArtifacts(params ArtifactName[] artifactNames)
    {
        List<Target> targets = [];

        foreach (var artifactName in artifactNames)
            if (Artifacts.TryGetValue(artifactName, out var artifact))
                targets.AddRange(artifact.Set.Targets.Values);
            else
                throw new ArgumentException($"can not found artifact `{artifactName}`");

        return BuildTargets(targets.Select(t => t.Name).ToArray());
    }

    public Task BuildTargets(params Name[] targetNames)
    {
        List<Task> tasks = new(targetNames.Length);
        Dictionary<Name, bool> searchStatus = [];
        Dictionary<Name, Task> searched = [];

        tasks
            .AddRange(targetNames
                .Select(artifact => TopologicalSorting
                    .Sort(Artifacts,
                        searchStatus,
                        searched,
                        Artifacts[artifact.ArtifactName].Set.Targets[artifact])));

        return Task.WhenAll(tasks);
    }

    public Task BuildTypedTargets(params TargetType[] targetTypes)
    {
        List<Target> targets = [];

        var artifacts = Artifacts.ToArray();

        foreach (var artifact in artifacts)
        {
            var typedTargets = artifact.Value.Set.TypedTargets;
            foreach (var targetType in targetTypes)
                if (typedTargets.TryGetValue(targetType, out var typedTarget))
                    targets.AddRange(typedTarget);
        }

        return BuildTargets(targets.Select(t => t.Name).ToArray());
    }
}
