using Microsoft.Extensions.Logging;
using Tenray.ZoneTree;
using Tenray.ZoneTree.Comparers;
using Tenray.ZoneTree.Options;
using Tenray.ZoneTree.Serializers;
using Tenray.ZoneTree.WAL;
using Vogen;

namespace ZMake.Api;

public class CacheDatabase<T> : IDisposable
{
    private IZoneTree<string, string> Database { get; }

    private IMaintainer Maintainer { get; }

    public CacheDatabase(string dataPath)
    {
        Database = new ZoneTreeFactory<string, string>()
            .SetComparer(new StringInvariantComparerAscending())
            .SetDataDirectory(dataPath)
            .SetKeySerializer(new Utf8StringSerializer())
            .SetValueSerializer(new Utf8StringSerializer())
            .ConfigureWriteAheadLogOptions((options =>
            {
                options.WriteAheadLogMode = WriteAheadLogMode.None;
            }))
            .OpenOrCreate();

        Maintainer = Database.CreateMaintainer();
        Maintainer.EnableJobForCleaningInactiveCaches = true;
        Maintainer.ThresholdForMergeOperationStart = 2;
    }

    public bool NeedUpdate(string path, T data)
    {
        if (Database.TryGet(path, out var value))
        {
            return value.Equals(data.ToString());
        }
        return true;
    }

    public void AddItem(string path, T data)
    {
        if (data == null) throw new ArgumentNullException(nameof(data));
        Database.Upsert(path, data.ToString());
    }

    public void SaveDatabase()
    {
        Maintainer.StartMerge();
        Maintainer.EvictToDisk();
    }

    public void Dispose()
    {
        Maintainer.WaitForBackgroundThreads();
        Maintainer.Dispose();
        Database.Dispose();
    }
}
