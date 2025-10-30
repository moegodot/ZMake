using System.Diagnostics;
using System.Formats.Tar;
using System.IO.Compression;
using ZMake.Api;
using ZMake.Api.BuiltIn;

namespace ZMake;

public static class EmbeddedResources
{
    public const string RepositoryResourceName = "zmake.tar.gz";
    public const string DotnetResourceName = "dotnet";

    public static async Task ExtractZMakeRepository(string destination)
    {
        await using var stream = typeof(EmbeddedResources).Assembly.GetManifestResourceStream(RepositoryResourceName)
        ?? throw new InvalidProgramException($"`{RepositoryResourceName}` not found in program's embedded resources");

        await using var archive = new GZipStream(stream, CompressionMode.Decompress);

        await TarFile.ExtractToDirectoryAsync(archive, destination, true);
    }

    public static async Task ExtractDotnet(string destination)
    {
        var isWindows = false;

        var os = Os.DetectedOs.Value;
        if (os is null)
        {
            throw new NotSupportedException("Not supported operation system");
        }

        if (os.Equals(Oses.Windows))
        {
            isWindows = true;
        }

        await using var stream = typeof(EmbeddedResources).Assembly.GetManifestResourceStream(DotnetResourceName)
            ?? throw new InvalidProgramException($"`{DotnetResourceName}` not found in program's embedded resources");

        if (isWindows)
        {
            ZipFile.ExtractToDirectory(stream, destination, true);
        }
        else
        {
            await using var archive = new GZipStream(stream, CompressionMode.Decompress);
            await TarFile.ExtractToDirectoryAsync(archive, destination, true);
        }
    }
}
