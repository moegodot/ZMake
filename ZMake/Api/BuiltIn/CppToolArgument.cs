namespace ZMake.Api.BuiltIn;

public record CppToolArgument : CToolArgument
{
    public CppToolArgument(bool enableDebug) : base(enableDebug)
    {
    }
}
