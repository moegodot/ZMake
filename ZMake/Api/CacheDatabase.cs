using Microsoft.Extensions.Logging;
using Tenray.ZoneTree;
using Tenray.ZoneTree.Comparers;
using Tenray.ZoneTree.Options;
using Tenray.ZoneTree.Serializers;
using Tenray.ZoneTree.WAL;
using Vogen;

namespace ZMake.Api;

public class CacheDatabase<TKey,TValue> : IDisposable
    where TValue:IEquatable<TValue>
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
                options.WriteAheadLogMode = WriteAheadLogMode.AsyncCompressed;
            }));

        build(factory);

        Database = factory.OpenOrCreate();

        Maintainer = Database.CreateMaintainer();
        Maintainer.EnableJobForCleaningInactiveCaches = true;
        Maintainer.ThresholdForMergeOperationStart = 2;
    }

    public static CacheDatabase<string, UInt128>
        CreateEmptyStrInt128(string dataDir)
    {
        return new CacheDatabase<string, UInt128>(dataDir, (factory =>
        {
            factory.SetComparer(new StringInvariantComparerAscending())
                .SetKeySerializer(new Utf8StringSerializer())
                .SetValueSerializer(new UInt128Serializer());
        }));
    }

    public bool CheckChanged(TKey path, TValue data)
    {
        if (Database.TryGet(path, out var value))
        {
            return value?.Equals(data) == false;
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
