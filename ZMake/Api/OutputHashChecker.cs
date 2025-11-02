using System.Text;
using Standart.Hash.xxHash;

namespace ZMake.Api;

public sealed class OutputHashChecker : IBuildChecker
{
    private readonly CacheDatabase<string, UInt128> _cacheDatabase;

    public OutputHashChecker(CacheDatabase<string,UInt128> database)
    {
        _cacheDatabase = database;
    }

    public async Task Update(BuildConstant build)
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

    public async Task<bool> CheckChanged(BuildConstant build)
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
