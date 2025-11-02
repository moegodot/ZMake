using System.Diagnostics;

namespace ZMake.Api;

public class ClangStyleCCompiler : Tool, IBuildTool<CToolArgument>
{
    public ClangStyleCCompiler(
        string program,
        ToolName name,
        ToolType type) : base(program, name, type)
    {
    }

    public async Task<bool> Build(
        IEnumerable<string> @in,
        IEnumerable<string> @out,
        CToolArgument arguments,
        string? workDir = null,
        IReadOnlyDictionary<string, string>? environment = null)
    {
        List<string> args = [];

        if (arguments.Optimization is not null)
        {
            args.Add(
                arguments.Optimization switch
                {
                    OptimizationLevel.None => "-O0",
                    OptimizationLevel.FavourSpeedMinimum => "-O1",
                    OptimizationLevel.FavourSpeedMedium => "-O2",
                    OptimizationLevel.FavourSpeedMaximum => "-O3",
                    OptimizationLevel.FavourSize => "-Os",
                    _ => throw new UnreachableException(),
                });
        }

        if (arguments.LanguageVersion is not null)
        {
            args.Add($"-std={arguments.LanguageVersion}");
        }

        if (arguments.UseUtf8.HasValue && arguments.UseUtf8.Value)
        {
            args.AddRange([
                    "-finput-charset=UTF-8",
                    "-fexec-charset=UTF-8"
                ]);
        }

        args.AddRange(arguments.Definitions.Select(def => $"-D{def}"));
        args.AddRange(@in);
        args.AddRange(@out.SelectMany(str => (IEnumerable<string>)[$"-o", str]));
        args.AddRange(arguments.AdditionalArguments);

        return await Call(args, workDir, environment);
    }
}
