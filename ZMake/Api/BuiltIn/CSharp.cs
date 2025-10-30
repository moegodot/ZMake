namespace ZMake.Api.BuiltIn;

public static class CSharp
{
    private static ToolName MakeBuiltinCompiler(ArtifactName version, string name)
    {
        return ToolName.From(Name.Create(version, $"tool.compiler.csharp.{name}"));
    }

    public static readonly ToolType Dotnet = ToolType.From(Name.Create(Version.V1V0V0, "tool.compiler.csharp"));


    public static readonly ToolName MsDotnet = MakeBuiltinCompiler(Version.V1V0V0, "dotnet");


    public static readonly ToolName Mono = MakeBuiltinCompiler(Version.V1V0V0, "mono");
}
