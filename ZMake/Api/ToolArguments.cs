namespace ZMake.Api;

public abstract class ToolArguments: IGetHashCode128
{
    public List<string> AdditionalArguments { get; set; } = [];

    public abstract UInt128 GetHashCode128();
}
