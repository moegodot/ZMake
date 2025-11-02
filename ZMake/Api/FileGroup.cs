namespace ZMake.Api;

public sealed class FileGroup
{
    public Dictionary<FileType, FileSet> Groups { get; } = [];
}
