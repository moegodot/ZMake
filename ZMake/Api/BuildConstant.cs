namespace ZMake.Api;

public class BuildConstant<T>
{
    public List<string> Sources { get; }
    public List<string> Outputs { get; }
    public Dictionary<string,string> Environments { get; }
    public T Options { get; }
    public IBuildTool<T> Tool { get; }

    public override string ToString()
    {
        return $"Build constant[sources:{string.Join(';',Sources)}][outputs:{string.Join(';',Outputs)}][options:{Options}][tool:{Tool}]";
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
               Tool.Equals(constant.Tool);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Sources.GetEnumerableHashCode(),
            Outputs.GetEnumerableHashCode(),
            Options,
            Tool);
    }
}
