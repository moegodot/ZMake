using System.Runtime.CompilerServices;

namespace ZMake.Api;

[AttributeUsage(
    AttributeTargets.Property,
    AllowMultiple = false,
    Inherited = false)]
public class CurrentDirectoryAttribute : Attribute
{
    public string Path { get; }
    public CurrentDirectoryAttribute([CallerFilePath] string path = null!)
    {
        ArgumentNullException.ThrowIfNull(path);
        Path = path;
    }
}
