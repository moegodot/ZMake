using JetBrains.Annotations;

namespace ZMake.Api;

[PublicAPI]
public interface ITarget
{
    Name Name { get; }

    IEnumerable<Name> Requirements { get; }

    IEnumerable<Task> Tasks { get; }
}
