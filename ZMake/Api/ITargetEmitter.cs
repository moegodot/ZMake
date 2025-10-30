using System.Collections.Concurrent;

namespace ZMake.Api;

public interface ITargetEmitter : ICacheable
{
    Name EmitterName { get; }

    IReadOnlyDictionary<Name, Target> PublicTargets { get; }
}
