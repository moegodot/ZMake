namespace ZMake.Api.BuiltIn;

public class DotnetArguments : ToolArguments
{
    public string? Command { get; set; } = "build";

    public string? Configuration { get; set; } = "Debug";

    public Dictionary<string, string> Properties { get; } = [];

    public DotnetArguments(string? command)
    {
        Command = command;
    }

    public DotnetArguments() { }

    public override UInt128 GetHashCode128()
    {
        return HashCode128.Combine(
            HashCode128.Get(Command ?? string.Empty),
            HashCode128.Get(Configuration ?? string.Empty),
            Properties.GetDictionaryHashCode128(),
            base.GetHashCode128());
    }
}
