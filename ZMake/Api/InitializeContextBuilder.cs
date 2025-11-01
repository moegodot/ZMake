using System.Collections.Frozen;

namespace ZMake.Api;

public sealed class InitializeContextBuilder
{
    public RootPathService PathService { get; set; }

    public ToolChainBuilder ToolChain { get; set; } = ToolChainBuilder.CreateFromEnvironment();

    public TaskEngine Engine { get; set; }

    public Dictionary<Name, Option> Options { get; set; } = [];

    public InitializeContextBuilder(RootPathService pathService, TaskEngine engine)
    {
        Engine = engine;
        PathService = pathService;
    }

    internal InitializeContext Build()
    {
        return new InitializeContext(
            PathService,
            Engine,
            ToolChain.Build(),
            new(Options.ToFrozenDictionary()));
    }
}
