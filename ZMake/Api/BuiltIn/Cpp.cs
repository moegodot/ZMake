namespace ZMake.Api.BuiltIn;

public static class Cpp
{
    private static ToolName MakeBuiltinCompiler(ArtifactName version, string name)
    {
        return ToolName.From(Name.Create(version, $"tool.compiler.cpp.{name}"));
    }

    public static readonly ToolType Compiler = ToolType.From(Name.Create(Version.V1V0V0, "tool.compiler.cpp"));
    public static readonly ToolName Gcc = MakeBuiltinCompiler(Version.V1V0V0, "gcc");
    public static readonly ToolName Clang = MakeBuiltinCompiler(Version.V1V0V0, "clang");
    public static readonly ToolName Msvc = MakeBuiltinCompiler(Version.V1V0V0, "msvc");

    public static IEnumerable<IBuildTool<CToolArgument>> TryFind(ToolName tool, FileFinder finder)
    {
        var target = "";

        if (tool.Equals(Gcc))
        {
            target = "g++";
        }

        if (tool.Equals(Clang))
        {
            target = "clang++";
        }

        if (tool.Equals(Msvc))
        {
            target = "cl";
        }

        return finder.Search(target).Select(s => new CppTool(s, tool, Compiler));
    }
}
