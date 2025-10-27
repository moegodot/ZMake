using Microsoft.Extensions.Logging;
using Tenray.ZoneTree;
using Tenray.ZoneTree.Comparers;
using Tenray.ZoneTree.Options;
using Tenray.ZoneTree.Serializers;
using Tenray.ZoneTree.WAL;
using Vogen;

namespace ZMake.Api;

public class CacheDatabase<TKey,TValue> : IDisposable
{
    public IZoneTree<TKey, TValue> Database { get; }

    private IMaintainer Maintainer { get; }

    public CacheDatabase(string dataPath,Action<ZoneTreeFactory<TKey, TValue>> build)
    {
        var factory = new ZoneTreeFactory<TKey, TValue>()
            //.SetComparer(new StringInvariantComparerAscending())
            .SetDataDirectory(dataPath)
            //.SetKeySerializer(new Utf8StringSerializer())
            //.SetValueSerializer(new Utf8StringSerializer())
            .ConfigureWriteAheadLogOptions((options =>
            {
                options.WriteAheadLogMode = WriteAheadLogMode.None;
            }));

        build(factory);

        Database = factory.OpenOrCreate();

        Maintainer = Database.CreateMaintainer();
        Maintainer.EnableJobForCleaningInactiveCaches = true;
        Maintainer.ThresholdForMergeOperationStart = 2;
    }

    public bool NeedUpdate(TKey path, TValue data)
    {
        if (Database.TryGet(path, out var value))
        {
            return value.Equals(data);
        }
        return true;
    }

    public void AddItem(TKey path, TValue data)
    {
        if (data == null) throw new ArgumentNullException(nameof(data));
        Database.Upsert(path, data);
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
