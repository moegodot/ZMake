namespace ZMake.Api;

public abstract class ArtifactsProviderBase : IArtifactsProvider
{
    protected abstract IEnumerable<IArtifact> GetProjects
        (InitializeContext context);

    public abstract void Initialize(InitializeContextBuilder builder);

    public void Initialize(InitializeContext context)
    {
        List<IArtifact> projects = GetProjects(context).ToList();
        List<Artifact> artifacts = [];

        foreach (var project in projects)
        {
            project.PreInitialize(context);
        }

        foreach (var project in projects)
        {
            project.Initialize(context);
        }

        foreach (var project in projects)
        {
            TargetSetBuilder targetSetBuilder = new();
            foreach (var t in projects
                         .SelectMany(proj => proj.GetTargets(context)
                             .SelectMany(source => source.GetTargets())))
            {
                targetSetBuilder.Add(t.Item1, t.Item2);
            }
            project.ConfigureTargetSet(context,ref targetSetBuilder);
            var targetSet = targetSetBuilder.Build();
            var artifact = new Artifact(project.Name, targetSet);
            artifacts.Add(artifact);
        }

        int index = 0;
        foreach (var project in projects)
        {
            project.EndInitialize(artifacts[index]);
            index++;
        }

        foreach (var artifact in artifacts)
        {
            if (context.Artifacts.TryAdd(artifact.Name, artifact))
            {
                throw new InvalidOperationException($"failed to add artifact `{artifact.Name}` to Context.Artifacts, it may be duplicated");
            }
        }
    }
}
