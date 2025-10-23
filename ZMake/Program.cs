using System.Reflection;
using Semver;

namespace ZMake;

public class Program
{

    private static readonly Version Version = Assembly.GetEntryAssembly()!.GetName().Version
                                              ?? throw new InvalidProgramException("no version found from entry assembly");

    public static readonly SemVersion SemVersion = SemVersion.FromVersion(Version);

    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
    }
}
