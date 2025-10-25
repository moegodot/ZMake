using System.Collections.Concurrent;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;

namespace ZMake.Api;

public sealed class BuildContext
{
    private readonly ILogger _logger;

    public BuildContext(ILoggerFactory loggerFactory, TaskEngine engine)
    {
        _logger = loggerFactory.CreateLogger(nameof(BuildContext));
        Engine = engine;
    }

    public long Id { get; } =
        ((long)RandomNumberGenerator.GetInt32(int.MinValue, int.MaxValue) << 32) |
        (uint)RandomNumberGenerator.GetInt32(int.MinValue, int.MaxValue);

    public TaskEngine Engine { get; }

    public ConcurrentDictionary<object, object> Items { get; } = [];

    private readonly CancellationTokenSource _tokenSource = new();

    public CancellationToken CancellationToken => _tokenSource.Token;

    public ConcurrentDictionary<ArtifactName, Artifact> Artifacts { get; } = [];

    public Task BuildArtifacts(params ArtifactName[] artifactNames)
    {
        List<ITarget> targets = [];

        foreach (var artifactName in artifactNames)
            if (Artifacts.TryGetValue(artifactName, out var artifact))
                targets.AddRange(artifact.Set.AllTargets.Values);
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
                        Artifacts[artifact.ArtifactName].Set.AllTargets[artifact])));

        return Task.WhenAll(tasks);
    }

    public Task BuildTypedTargets(params TargetType[] targetTypes)
    {
        List<ITarget> targets = [];

        var artifacts = Artifacts.ToArray();

        foreach (var artifact in artifacts)
        {
            var typedTargets = artifact.Value.Set.TypedTargets;
            foreach (var targetType in targetTypes)
                if (typedTargets.TryGetValue(targetType, out var typedTarget))
                    targets.Add(typedTarget);
        }

        return BuildTargets(targets.Select(t => t.Name).ToArray());
    }
}
