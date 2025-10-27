using System.Diagnostics;
using Semver;

namespace ZMake.Api;

public static class VersionExtractor
{
    /// <summary>
    /// Extract version from the output of program.
    /// </summary>
    public static SemVersion? TryExtractVersion(string output)
    {
        var outputs = output.Trim().Split();

        foreach (var item in outputs)
        {
            if (SemVersion.TryParse(item,
                    SemVersionStyles.AllowV |
                    SemVersionStyles.AllowWhitespace |
                    SemVersionStyles.OptionalPatch,
                    out var semver))
            {
                return semver;
            }
        }

        return null;
    }

    public static async Task<SemVersion?> TryGetVersionOfProgram(string program,string argument)
    {
        var proc = new Process();
        proc.StartInfo.FileName = program;
        proc.StartInfo.Arguments = argument;
        proc.StartInfo.UseShellExecute = false;
        proc.StartInfo.RedirectStandardOutput = true;
        proc.StartInfo.RedirectStandardError = true;
        proc.StartInfo.RedirectStandardInput = true;

        if (!proc.Start())
        {
            throw new InvalidOperationException($"failed to start program `{program}`");
        }

        proc.StandardInput.Close();

        await proc.WaitForExitAsync();

        return TryExtractVersion(proc.StandardOutput.ReadToEnd())
               ?? TryExtractVersion(proc.StandardError.ReadToEnd());
    }

    public static async Task<SemVersion?> TryGetVersionOfProgram(string program)
    {
        return await TryGetVersionOfProgram(program, "--version")
               ??
               await TryGetVersionOfProgram(program, "-v")
               ??
               await TryGetVersionOfProgram(program, "/v")
               ??
               await TryGetVersionOfProgram(program, "");
    }
}
