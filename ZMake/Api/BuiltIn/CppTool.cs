namespace ZMake.Api.BuiltIn;

public class CppTool : ClangStyleCCompiler
{
    public CppTool(string program
        ,ToolName name,
        ToolType type) : base(program,name,type)
    {
    }
}
