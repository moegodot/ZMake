using System.Security.Cryptography;

namespace ZMake.Api.BuiltIn;

public sealed class SourceHashChanged(
    string[] source,
    string[] output,
    CacheDatabase<string, byte[]> database) : INotifyBuildChanged
{
    public CacheDatabase<string, byte[]> HashDataBase { get; } = database;
    public string[] Source { get; } = source;
    public string[] Output { get; } = output;

    public bool IsChanged
    {
        get
        {
            foreach (var source in Source)
            {
                var content = File.ReadAllBytes(source);
                return HashDataBase.NeedUpdate(source,
                    SHA256.HashData(content));
            }

            return false;
        }
    }
}
