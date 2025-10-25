namespace ZMake.Api;

public interface ITarget
{
    Name Name { get; }

    IEnumerable<Name> Requirements { get; }

    IEnumerable<Task> Tasks { get; }
}
