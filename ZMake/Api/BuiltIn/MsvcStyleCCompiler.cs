using System.Diagnostics;

namespace ZMake.Api.BuiltIn;

public class MsvcStyleCCompiler: Tool,IBuildTool<CToolArgument>
{
    public MsvcStyleCCompiler(
        string program,
        ToolName name,
        ToolType type) : base(program,name,type)
    {
    }

    public async Task<bool> Build(
        IEnumerable<string> @in,
        IEnumerable<string> @out,
        string workDir,
        Dictionary<string, string> environment,
        CToolArgument arguments)
    {
        List<string> args = ["/nologo"];

        if (arguments.Optimization is not null)
        {
            args.Add(
                arguments.Optimization switch {
                        OptimizationLevel.None => "/Od",
                        OptimizationLevel.FavourSpeedMinimum => "/Ot",
                        OptimizationLevel.FavourSpeedMedium => "/Ox",
                        OptimizationLevel.FavourSpeedMaximum => "/O2",
                        OptimizationLevel.FavourSize => "/O1",
                        _ => throw new UnreachableException(),
                    });
        }

        if (arguments.LanguageVersion is not null)
        {
            args.Add($"/std={arguments.LanguageVersion}");
        }

        if (arguments.Permissive.HasValue && (!arguments.Permissive.Value))
        {
            args.Add("/permissive-");
            args.Add("/Zc");
        }

        if (arguments.UseUtf8.HasValue && arguments.UseUtf8.Value)
        {
            args.Add("/utf-8");
        }

        args.AddRange(arguments.Definitions.Select(def => $"/D{def}"));
        args.AddRange(@in);
        args.AddRange(@out.SelectMany(str => (IEnumerable<string>)[$"/o",str]));
        args.AddRange(arguments.AdditionalArguments);

        return await Call(workDir, environment, args);
    }
}
