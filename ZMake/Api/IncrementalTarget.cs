namespace ZMake.Api;

public class IncrementalTarget(
    INotifyBuildChanged changed,
    Name name,
    IEnumerable<Name> requirements,
    IEnumerable<Task> tasks)
    : ITarget
{
    public INotifyBuildChanged Changed { get; } = changed;
    public Name Name { get; } = name;
    public IEnumerable<Name> Requirements { get; } = requirements;
    public IEnumerable<Task> Tasks { get; } =
    [
        new Task(() =>
        {
            if (changed.IsChanged)
            {
                Task.WhenAll(tasks).Wait();
            }
        })
    ];
}
