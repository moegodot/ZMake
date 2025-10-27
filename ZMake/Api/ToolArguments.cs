namespace ZMake.Api;

public record ToolArguments
{
    public List<string> AdditionalArguments { get; set; } = [];
}
