using System.Collections.Frozen;
using System.Collections.Immutable;

namespace ZMake.Api;

public class BuildConstantBuilder
{
    public BuildConstantBuilder(IContext context)
    {
        ToolChain = context.ToolChain;
        WorkDir = context.PathService.RootBinaryObjectsPath;
    }

    public ToolChain ToolChain { get; }
    public List<string> Sources { get; set; } = [];
    public List<string> Outputs { get; set; } = [];
    public Dictionary<string, string>? Environments { get; set; } = null;
    public List<ToolArguments> Options { get; set; } = [];
    public List<ITool> Tools { get; set; } = [];
    public string WorkDir { get; set; }

    public BuildConstantBuilder WithSources(params IEnumerable<string> sources)
    {
        Sources.AddRange(sources);
        return this;
    }

    public BuildConstantBuilder WithOutputs(params IEnumerable<string> outputs)
    {
        Outputs.AddRange(outputs);
        return this;
    }

    public BuildConstantBuilder WithEnvironments(Dictionary<string, string> environments)
    {
        Environments = environments;
        return this;
    }

    public BuildConstantBuilder WithOptions(ToolArguments options)
    {
        Options.Add(options);
        return this;
    }
    public BuildConstantBuilder WithTool(ITool tool)
    {
        Tools.Add(tool);
        return this;
    }

    public BuildConstantBuilder WithWorkDir(string workDir)
    {
        WorkDir = workDir;
        return this;
    }

    public BuildConstant Build()
    {
        return new BuildConstant(
        Sources.ToImmutableArray(),
        Outputs.ToImmutableArray(),
        Environments?.ToFrozenDictionary() ?? ToolChain.Environments,
        Options.ToImmutableArray(),
        Tools.ToImmutableArray(),
        WorkDir);
    }
}
