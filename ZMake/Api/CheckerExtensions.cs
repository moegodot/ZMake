namespace ZMake.Api;

public static class CheckerExtensions
{
    public static OutputHashChecker CreateOutputHashChecker(this IContext context)
    {
        var cache = $"{context.PathService.RootCachePath}/{nameof(ZMake)}.{nameof(Api)}.{typeof(OutputHashChecker)}";
        var dataBase = CacheDatabase<string, UInt128>.CreateEmptyStrInt128(cache);
        return new OutputHashChecker(dataBase);
    }

    public static SourceHashChecker CreateSourceHashChecker(this IContext context)
    {
        var cache = $"{context.PathService.RootCachePath}/{nameof(ZMake)}.{nameof(Api)}.{typeof(SourceHashChecker)}";
        var dataBase = CacheDatabase<string, UInt128>.CreateEmptyStrInt128(cache);
        return new SourceHashChecker(dataBase);
    }
}
