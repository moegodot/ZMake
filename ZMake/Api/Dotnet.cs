namespace ZMake.Api;

public class Dotnet : Tool, IBuildTool<DotnetArguments>
{
    public Dotnet(string program, ToolName name, ToolType type) : base(program, name, type)
    {
    }

    public async Task<bool> Build(
        IEnumerable<string> @in,
        IEnumerable<string> @out,
        DotnetArguments arguments,
        string? workDir = null,
        IReadOnlyDictionary<string, string>? environment = null)
    {
        List<string> args = [];

        if (arguments.Command is not null)
        {
            args.Add(arguments.Command);
        }

        if (arguments.Configuration is not null)
        {
            args.Add($"--configuration={arguments.Configuration}");
        }

        args.AddRange(@in);
        args.AddRange(@out.SelectMany(str => (IEnumerable<string>)[$"-o", str]));
        args.AddRange(arguments.Properties.Select(prop => $"--property:{prop.Key}={prop.Value}"));
        args.AddRange(arguments.AdditionalArguments);

        return await Call(args, workDir, environment);
    }
}
