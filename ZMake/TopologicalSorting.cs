using ZMake.Api;

namespace ZMake;

internal static class TopologicalSorting
{
    public static Task Sort(
        IReadOnlyDictionary<ArtifactName, Artifact> targets,
        Dictionary<Name, bool> searchStatus,
        Dictionary<Name, Task> searched,
        Target target)
    {
        var found = searchStatus.TryGetValue(target.Name, out var inSearching);
        if (found && inSearching) throw new ArgumentException($"find circular dependency in searching of {target.Name}");
        if (found) return searched[target.Name];

        searchStatus[target.Name] = true;

        var requirements = target.PublicDependencies.ToArray();
        List<Task> parentTasks = new(requirements.Length);

        foreach (var requirementName in requirements)
        {
            if (targets.TryGetValue(requirementName.ArtifactName, out var requirementArtifact)
                && requirementArtifact.Set.Targets.TryGetValue(requirementName, out var requirement))
            {
                try
                {
                    parentTasks.Add(Sort(targets, searchStatus, searched, requirement));
                }
                catch (Exception exception)
                {
                    throw new AggregateException($"get an exception in searching of {target.Name}", exception);
                }
            }
            else
                throw new InvalidOperationException(
                    $"can not find requirement `{requirementName}` of target `{target}` in all targets");
        }

        foreach (var privateDependency in target.PrivateDependencies)
        {
            try
            {
                parentTasks.Add(Sort(targets, searchStatus, searched, privateDependency));
            }
            catch (Exception exception)
            {
                throw new AggregateException($"get an exception in searching of {target.Name}", exception);
            }
        }

        searchStatus[target.Name] = false;
        var task =
        new Task<Task>(async () => await Task.WhenAll(parentTasks))
        .ContinueWith(
            async preTasks =>
            {
                if (!preTasks.IsCompletedSuccessfully)
                {
                    await preTasks;
                }
                return Task.WhenAll(target.Tasks);
            });
        searched[target.Name] = task;

        return task;
    }
}
