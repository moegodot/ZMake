namespace ZMake;

public static class TopologicalSorting
{
    public static Task Sort(
        IReadOnlyDictionary<ArtifactName, Artifact> targets,
        Dictionary<Name, bool> searchStatus,
        Dictionary<Name, Task> searched,
        ITarget target)
    {
        var found = searchStatus.TryGetValue(target.Name, out var inSearch);
        if (found && inSearch)
        {
            throw new ArgumentException($"find circular dependency in searching of {target.Name}");
        }
        if (found)
        {
            return searched[target.Name];
        }

        searchStatus[target.Name] = true;

        var requirements = target.Requirements.ToArray();
        List<Task> parentTasks = new(requirements.Length);

        foreach (var requirementName in requirements)
        {
            if (targets.TryGetValue(requirementName.ArtifactName,out var requirementArtifact)
                && requirementArtifact.Set.AllTargets.TryGetValue(requirementName, out var requirement))
            {
                try
                {
                    parentTasks.Add(Sort(targets, searchStatus,searched, requirement));
                }
                catch (Exception exception)
                {
                    throw new AggregateException($"get an exception in searching of {target.Name}", exception);
                }
            }
            else
            {
                throw new InvalidOperationException($"can not find requirement `{requirementName}` of target `{target}` in all targets");
            }
        }

        searchStatus[target.Name] = false;
        var task = Task.WhenAll([..target.Tasks, ..parentTasks]);
        searched[target.Name] = task;

        return task;
    }
}
