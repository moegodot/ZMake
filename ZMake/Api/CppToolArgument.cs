namespace ZMake.Api;

public class CppToolArgument : CToolArgument
{
    public CppToolArgument(bool enableDebug) : base(enableDebug)
    {
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(nameof(CppToolArgument), base.GetHashCode());
    }

    public override UInt128 GetHashCode128()
    {
        return HashCode128.Combine(
            HashCode128.Get(nameof(CppToolArgument)),
            base.GetHashCode128());
    }
}
