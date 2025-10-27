namespace ZMake.Api.BuiltIn;

public static class C
{
    private static ToolName MakeBuiltinCompiler(ArtifactName version, string name)
    {
        return ToolName.From(Name.Create(version, ["tool","compiler","c", name]));
    }

    public static readonly ToolType Compiler = ToolType.From(Name.Create(Version.V1V0V0, "tool","compiler","c"));
    public static readonly ToolName Gcc = MakeBuiltinCompiler(Version.V1V0V0, "gcc");
    public static readonly ToolName Clang = MakeBuiltinCompiler(Version.V1V0V0, "clang");
    public static readonly ToolName Msvc = MakeBuiltinCompiler(Version.V1V0V0, "msvc");

    public static IEnumerable<IBuildTool<CToolArgument>> Find(ToolName tool,FileFinder finder)
    {
        var target = "clang";

        if (tool.Equals(Gcc))
        {
            target = "gcc";
        }

        if (tool.Equals(Clang))
        {
            target = "clang";
        }

        if (tool.Equals(Msvc))
        {
            target = "cl";
        }

        if (target.Equals("cl")){
            return finder.Search(target).Select(s => new MsvcStyleCCompiler(s, tool, Compiler));
        }
        return finder.Search(target).Select(s => new ClangStyleCCompiler(s, tool, Compiler));
    }
}
