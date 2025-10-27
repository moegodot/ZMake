using System.Diagnostics;

namespace ZMake.Api.BuiltIn;

public static class VsWhere
{
    public static readonly Lazy<string> Executable = new(() =>
        $"{Environment.GetEnvironmentVariable("ProgramFiles(x86)")}/Microsoft Visual Studio/Installer/vswhere.exe",
        LazyThreadSafetyMode.None);

    public static string GetInstallPath(string requiredComponent)
    {
        using Process process = new();
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardInput = true;
        process.StartInfo.FileName = Executable.Value;
        process.StartInfo.ArgumentList.Add("-latest");
        process.StartInfo.ArgumentList.Add("-products");
        process.StartInfo.ArgumentList.Add("*");
        process.StartInfo.ArgumentList.Add("-requires");
        process.StartInfo.ArgumentList.Add(requiredComponent);
        process.StartInfo.ArgumentList.Add("-property");
        process.StartInfo.ArgumentList.Add("installationPath");
        process.Start();
        process.StandardInput.Close();
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException($"vswhere.exe exit with code {process.ExitCode}");
        }

        return process.StandardOutput.ReadToEnd().Trim();
    }

    public static string GetVcDevCmdBatForX64()
    {
        var path = GetInstallPath("Microsoft.VisualStudio.Component.VC.Tools.x86.x64");
        return Path.Combine(path, "Common7/Tools/VsDevCmd.bat");
    }

    public static string GetVcDevCmdBatForArm64()
    {
        var path = GetInstallPath("Microsoft.VisualStudio.Component.VC.Tools.ARM64");
        return Path.Combine(path, "Common7/Tools/VsDevCmd.bat");
    }

    public static async Task<Dictionary<string, string>>
        GetBatEnvironment(
        string bat,
        string hostArch,
        string arch)
    {
        using Process process = new();
        process.StartInfo.FileName = "cmd.exe";
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardInput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.ArgumentList.Add("/c");
        process.StartInfo.ArgumentList.Add($"\"{bat}\" -no_logo -host_arch={hostArch} -arch={arch} & SET");
        process.StartInfo.Environment.Clear();
        process.StartInfo.EnvironmentVariables.Clear();
        if (!process.Start())
        {
            throw new InvalidOperationException($"Can not start process:{bat}");
        }

        process.StandardInput.Close();
        await process.WaitForExitAsync();

        var str = await process.StandardOutput.ReadToEndAsync();
        var env = new Dictionary<string, string>();
        var dictArray = str.Replace("\r\n","\n").Replace("\r","\n").Split(['=', '\n']);
        var index = 0;

        while (index != dictArray.Length)
        {
            env[dictArray[index]] = env[dictArray[index + 1]];
            index += 2;
        }

        return env;
    }
}
