using System.Security.Cryptography;
using System.Text;
using Standart.Hash.xxHash;

namespace ZMake.Api.BuiltIn;

public sealed class OutputHashChecker<T> : IBuildChecker<T> where T : ToolArguments
{
    private readonly CacheDatabase<string, UInt128> _cacheDatabase;
    public OutputHashChecker(CacheDatabase<string,UInt128> database)
    {
        _cacheDatabase = database;
    }
    public async Task Update(BuildConstant<T> build)
    {
        foreach (var output in build.Outputs)
        {
            if (!File.Exists(output))
            {
                continue;
            }
            var hash = xxHash128.ComputeHash(
                    await File.ReadAllTextAsync(output, Encoding.UTF8))
                .ToUint128();
            _cacheDatabase.AddItem(output, hash);
        }
    }

    public async Task<bool> CheckChanged(BuildConstant<T> build)
    {
        foreach (var output in build.Outputs)
        {
            if (!File.Exists(output))
            {
                return true;
            }

            var hash = xxHash128.ComputeHash(
                    await File.ReadAllTextAsync(output, Encoding.UTF8))
                .ToUint128();

            if (_cacheDatabase.CheckChanged(output, hash))
            {
                return true;
            }
        }

        return false;
    }
}
