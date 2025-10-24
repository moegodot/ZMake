using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Pillar.Demystifier;
using Semver;
using StrongInject;

namespace ZMake;

public class Program
{

    private static readonly Version Version = Assembly.GetEntryAssembly()!.GetName().Version
                                              ?? throw new InvalidProgramException("no version found from entry assembly");

    public static readonly SemVersion SemVersion = SemVersion.FromVersion(Version);

    public static async Task<int> Main(string[] args)
    {
        try
        {
            var factory = LoggerFactory.Create((builder) => { });
            var container = new Container(factory);
            using var context = container.Resolve();

        }
        catch (Exception exception)
        {
            exception.PrintColoredStringDemystified(StyledBuilderOption.GlobalOption, Console.Error);
            return 1;
        }

        return 0;
    }
}
