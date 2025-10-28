using System.Security.Cryptography;
using System.Text;
using Standart.Hash.xxHash;

namespace ZMake.Api.BuiltIn;

public sealed class SourceHashChecker<T> : IBuildChecker<T> where T : ToolArguments
{
    private readonly CacheDatabase<string, UInt128> _cacheDatabase;
    public SourceHashChecker(CacheDatabase<string,UInt128> database)
    {
        _cacheDatabase = database;
    }
    public async Task Update(BuildConstant<T> build)
    {
        foreach (var source in build.Sources)
        {
            if (!File.Exists(source))
            {
                continue;
            }
            var hash = xxHash128.ComputeHash(
                    await File.ReadAllTextAsync(source, Encoding.UTF8))
                .ToUint128();
            _cacheDatabase.AddItem(source, hash);
        }
    }

    public async Task<bool> CheckChanged(BuildConstant<T> build)
    {
        foreach (var source in build.Sources)
        {
            if (!File.Exists(source))
            {
                return true;
            }

            var hash = xxHash128.ComputeHash(
                    await File.ReadAllTextAsync(source, Encoding.UTF8))
                .ToUint128();

            if (_cacheDatabase.CheckChanged(source, hash))
            {
                return true;
            }
        }

        return false;
    }
}
