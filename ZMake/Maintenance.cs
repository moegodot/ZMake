
using System.Text;
using ZMake.Api;

namespace ZMake;

public static class Maintenance
{
    public const string ZMakeLockFileName = "zmake.build.lock";

    public const string GitRepository = "https://github.com/moegodot/ZMake";

    public static Task CreateBuildDirectory(string sourceDirectory, string buildDirectory)
    {
        sourceDirectory = Path.GetFullPath(sourceDirectory);
        buildDirectory = Path.GetFullPath(buildDirectory);

        if (Directory.Exists(buildDirectory))
        {
            throw new ArgumentException("The build directory of the repository existed.");
        }

        Directory.CreateDirectory(buildDirectory);

        var zmakeLockFile = Path.Combine(sourceDirectory, ZMakeLockFileName);
        var writeLockFile = File.WriteAllTextAsync(zmakeLockFile, Path.GetRelativePath(sourceDirectory, buildDirectory), Encoding.UTF8);

        var objDir = Path.Combine(buildDirectory, "obj");
        Directory.CreateDirectory(objDir);

        var zmakeDir = Path.Combine(buildDirectory, RootPathService.ZMakeProjectDirectoryName);
        Directory.CreateDirectory(zmakeDir);
        var zmakeRepositoryTask = EmbeddedResources.ExtractZMakeRepository(zmakeDir);

        var dotnetDir = Path.Combine(buildDirectory, RootPathService.DotnetDirectoryName);
        Directory.CreateDirectory(dotnetDir);
        var dotnetTask = EmbeddedResources.ExtractDotnet(dotnetDir);

        return Task.WhenAll(zmakeRepositoryTask, dotnetTask, writeLockFile);
    }

    public static (string source, string build)? SearchSourceAndBuildDirectory(string directory = ".")
    {
        while (true)
        {
            directory = Path.GetFullPath(directory);
            var file = Path.Combine(directory, ZMakeLockFileName);

            if (File.Exists(file))
            {
                return (directory,
                    Path.Combine(directory,
                        File.ReadAllText(file, Encoding.UTF8)));
            }

            var parent = Path.GetDirectoryName(directory);

            if (parent == null || parent == directory)
            {
                return null;
            }

            directory = parent;
        }
    }
}
