using System.Collections.Immutable;

namespace ZMake.Api;

public class Target
{
    public Target(
        Name name,
        AccessibilityLevel accessibilityLevel,
        ImmutableArray<Target> privateDependencies,
        ImmutableArray<Name> publicDependencies,
        ImmutableArray<Task<Task>> tasks)
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
    public ImmutableArray<Task<Task>> Tasks { get; }
    public override string ToString()
    {
        return $"(Target {Name})";
    }
}
