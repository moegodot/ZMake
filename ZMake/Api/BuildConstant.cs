namespace ZMake.Api;

public sealed class BuildConstant<T>:IGetHashCode128 where T:ToolArguments
{
    public List<string> Sources { get; }
    public List<string> Outputs { get; }
    public Dictionary<string,string> Environments { get; }
    public T Options { get; }
    public IBuildTool<T> Tool { get; }
    public string WorkDir { get; }

    public BuildConstant(
        List<string> sources,
        List<string> outputs,
        Dictionary<string,string> environments,
        T options,
        IBuildTool<T> tool,
        string workDir)
    {
        Sources = sources;
        Outputs = outputs;
        Environments = environments;
        Options = options;
        Tool = tool;
        WorkDir = workDir;
    }

    public override string ToString()
    {
        return $"Build constant[sources:{string.Join(';',Sources)}][outputs:{string.Join(';',Outputs)}][workdir:{WorkDir}][options:{Options}][tool:{Tool}]";
    }

    public UInt128 GetHashCode128()
    {
        return HashCode128.Combine(
            Sources.GetEnumerableHashCode128(),
            Outputs.GetEnumerableHashCode128(),
            Environments.GetDictionaryHashCode128(),
            Options.GetHashCode128(),
            Tool.GetHashCode128(),
            HashCode128.Get(WorkDir));
    }

    public override bool Equals(object? obj)
    {
        if (obj is not BuildConstant<T> constant)
        {
            return false;
        }

        return Sources.SequenceEqual(constant.Sources) &&
               Outputs.SequenceEqual(constant.Outputs) &&
               Options.Equals(constant.Options) &&
               Environments.DictionaryEquals(constant.Environments) &&
               Tool.Equals(constant.Tool) &&
               WorkDir.Equals(constant.WorkDir);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            Sources.GetEnumerableHashCode(),
            Outputs.GetEnumerableHashCode(),
            Environments.GetDictionaryHashCode(),
            Options,
            Tool,
            WorkDir);
    }
}
