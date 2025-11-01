using System.Collections.Immutable;

namespace ZMake.Api;

public sealed class Target
{
    public Target(
        Name name,
        AccessibilityLevel accessibilityLevel,
        ImmutableArray<Target> privateDependencies,
        ImmutableArray<Name> publicDependencies,
        ImmutableArray<Func<Task>> tasks)
    {
        Name = name;
        PrivateDependencies = privateDependencies;
        PublicDependencies = publicDependencies;
        Tasks = tasks;
        AccessibilityLevel = accessibilityLevel;
    }

    public Name Name { get; }
    public AccessibilityLevel AccessibilityLevel { get; }
    public ImmutableArray<Target> PrivateDependencies { get; }
    public ImmutableArray<Name> PublicDependencies { get; }
    public ImmutableArray<Func<Task>> Tasks { get; }

    public Task Run()
    {
        List<Task> tasks = new(Tasks.Length);

        foreach (var task in Tasks)
        {
            tasks.Add(task());
        }

        return Task.WhenAll(tasks);
    }

    public override string ToString()
    {
        return $"(Target {Name})";
    }
}
