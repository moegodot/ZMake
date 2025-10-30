namespace ZMake.Api;

public abstract class ToolArguments : IGetHashCode128
{
    public List<string> AdditionalArguments { get; set; } = [];

    public virtual UInt128 GetHashCode128()
    {
        return HashCode128.Combine(
            HashCode128.Get(nameof(ToolArguments)),
            AdditionalArguments.GetEnumerableHashCode128());
    }
}
