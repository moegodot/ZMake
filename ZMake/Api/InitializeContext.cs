using System.Collections.Frozen;
using System.Collections.Immutable;
using Pillar.Event;

namespace ZMake.Api;

public sealed partial class InitializeContext : IContext
{
    [EmitEvent]
    private readonly ListEvent<InitializeContext, InitializedContextEventArgs> _initializedContext = new();

    public Dictionary<ArtifactName, Artifact> Artifacts { get; } = [];

    public RootPathService PathService { get; }

    public ToolChain ToolChain { get; }

    public TaskEngine TaskEngine { get; }

    public OptionProvider OptionProvider { get; }

    internal InitializeContext(
        RootPathService pathService,
        TaskEngine taskEngine,
        ToolChain toolChain,
        OptionProvider provider)
    {
        TaskEngine = taskEngine;
        PathService = pathService;
        ToolChain = toolChain;
        OptionProvider = provider;
    }

    internal BuildContext Build()
    {
        var context = new BuildContext()
        {
            Artifacts = Artifacts.ToFrozenDictionary(),
            PathService = PathService,
            TaskEngine = TaskEngine,
            ToolChain = ToolChain,
        };

        _initializedContext.Fire(this, new()
        {
            Context = context
        });

        return context;
    }
}
