using System.Collections.Frozen;
using System.Collections.Immutable;
using M31.FluentApi.Attributes;

namespace ZMake.Api;

[FluentApi]
public sealed class BuildConstant:IGetHashCode128
{
    [FluentMember(0)]
    public ImmutableArray<string> Sources { get; init; }
    [FluentMember(1)]
    public ImmutableArray<string> Outputs { get;init; }
    [FluentMember(2)]
    public FrozenDictionary<string,string> Environments { get;init; }
    [FluentMember(3)]
    public ImmutableArray<ToolArguments> Options { get;init; }
    [FluentMember(5)]
    public ImmutableArray<ITool> Tools { get;init; }
    [FluentMember(6)]
    public string WorkDir { get;init; }

    public BuildConstant(
        ImmutableArray<string> sources,
        ImmutableArray<string> outputs,
        FrozenDictionary<string,string> environments,
        ImmutableArray<ToolArguments> options,
        ImmutableArray<ITool> tools,
        string workDir)
    {
        Sources = sources;
        Outputs = outputs;
        Environments = environments;
        Options = options;
        Tools = tools;
        WorkDir = workDir;
    }

    public override string ToString()
    {
        return $"Build constant[sources:{string.Join(';',Sources)}][outputs:{string.Join(';',Outputs)}][workdir:{WorkDir}][options:{Options}][tool:{Tools}]";
    }

    public UInt128 GetHashCode128()
    {
        return HashCode128.Combine(
            Sources.GetEnumerableHashCode128(),
            Outputs.GetEnumerableHashCode128(),
            Environments.GetDictionaryHashCode128(),
            Options.GetEnumerableHashCode128(),
            Tools.GetEnumerableHashCode128(),
            HashCode128.Get(WorkDir));
    }

    public override bool Equals(object? obj)
    {
        if (obj is not BuildConstant constant)
        {
            return false;
        }

        return Sources.SequenceEqual(constant.Sources) &&
               Outputs.SequenceEqual(constant.Outputs) &&
               Options.SequenceEqual(constant.Options) &&
               Environments.DictionaryEquals(constant.Environments) &&
               Tools.SequenceEqual(constant.Tools)  &&
               WorkDir.Equals(constant.WorkDir);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            Sources.GetEnumerableHashCode(),
            Outputs.GetEnumerableHashCode(),
            Environments.GetDictionaryHashCode(),
            Options,
            Tools,
            WorkDir);
    }
}
