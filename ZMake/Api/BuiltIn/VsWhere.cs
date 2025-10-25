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
            throw new InvalidOperationException("vswhere.exe exit with code non-zero");
        }

        return process.StandardOutput.ReadToEnd().Trim();
    }

    public static string GetX64Tool()
    {
        return GetInstallPath("Microsoft.VisualStudio.Component.VC.Tools.x86.x64");
    }

    public static string GetArm64Tool()
    {
        return GetInstallPath("Microsoft.VisualStudio.Component.VC.Tools.ARM64");
    }
}
