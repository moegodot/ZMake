using System.Collections.Immutable;
using System.Diagnostics.Contracts;

namespace ZMake.Api.BuiltIn;

public sealed class TargetBuilder : ITargetSource
{
    public Name? TargetName { get; set; } = null;

    public TargetType? TargetType { get; set; } = null;

    public AccessibilityLevel AccessibilityLevel { get; set; } = AccessibilityLevel.Private;

    public List<Target> PrivateDependencies { get; } = [];

    public List<Name> PublicDependencies { get; } = [];

    public List<Func<Task>> Tasks { get; } = [];

    public Action<Target>? BuildAction { get; set; } = null;

    public Target? BuiltTarget { get; private set; } = null;

    private void CheckBuilt()
    {
        if (BuiltTarget is not null)
        {
            throw new InvalidOperationException("The target has been built");
        }
    }

    public TargetBuilder WithName(Name targetName)
    {
        CheckBuilt();
        TargetName = targetName;
        return this;
    }

    public TargetBuilder WithPrivateDependencies(IEnumerable<Target> privateDependencies)
    {
        CheckBuilt();
        PrivateDependencies.AddRange(privateDependencies);
        return this;
    }

    public TargetBuilder WithPrivateDependencies(Target privateDependencies)
    {
        CheckBuilt();
        PrivateDependencies.Add(privateDependencies);
        return this;
    }

    public TargetBuilder WithPublicDependencies(IEnumerable<Name> publicDependencies)
    {
        CheckBuilt();
        PublicDependencies.AddRange(publicDependencies);
        return this;
    }

    public TargetBuilder WithPublicDependencies(Name publicDependencies)
    {
        CheckBuilt();
        PublicDependencies.Add(publicDependencies);
        return this;
    }

    public TargetBuilder WithTasks(IEnumerable<Func<Task>> tasks)
    {
        CheckBuilt();
        Tasks.AddRange(tasks);
        return this;
    }

    public TargetBuilder Public(TargetType? targetType)
    {
        CheckBuilt();
        TargetType = targetType;
        AccessibilityLevel = AccessibilityLevel.Public;
        return this;
    }

    public TargetBuilder PrivateAndClearType()
    {
        CheckBuilt();
        TargetType = null;
        AccessibilityLevel = AccessibilityLevel.Private;
        return this;
    }

    public TargetBuilder WithTask(Func<Task> task)
    {
        CheckBuilt();
        Tasks.Add(task);
        return this;
    }

    public TargetBuilder WithTask(Task task)
    {
        CheckBuilt();
        Tasks.Add(
            () => task
        );
        return this;
    }

    public TargetBuilder WithBuildToolCall<T>(ToolChain toolChain, ToolType toolType, IEnumerable<string> @in, IEnumerable<string> @out, T arguments) where T : ToolArguments
    {
        CheckBuilt();
        Tasks.Add(
            () =>
            {
                return ((IBuildTool<T>?)toolChain[toolType] ?? throw new InvalidOperationException($"Failed to find the program `{toolType}`")).Build(@in, @out, arguments, null, toolChain.Environments);
            }
        );
        return this;
    }

    public TargetBuilder WithBuildAction(Action<Target> action)
    {
        CheckBuilt();
        BuildAction = action;
        return this;
    }

    public TargetBuilder WithBuildActionWriteTo(Dictionary<Name, Target> targets)
    {
        CheckBuilt();
        return WithBuildAction((target) => targets.Add(target.Name, target));
    }

    public TargetBuilder OutName(out Name name)
    {
        name = TargetName ?? throw new InvalidOperationException("Target name is not set");
        return this;
    }

    /// <summary>
    /// Out the target,this will also build the target if it is not built yet.
    /// </summary>
    /// <param name="target">The target to be built</param>
    /// <exception cref="InvalidOperationException">When the target is not public or the target type is set; When the target name is not set</exception>
    public void OutTarget(out Target target)
    {
        if (AccessibilityLevel == AccessibilityLevel.Private
            && TargetType is not null)
        {
            throw new InvalidOperationException("When accessibility level is private,target type must be null");
        }

        if (BuiltTarget is not null)
        {
            target = BuiltTarget;
            return;
        }

        BuiltTarget = new Target(
            TargetName ?? throw new InvalidOperationException("Target name is not set"),
            AccessibilityLevel,
            PrivateDependencies.ToImmutableArray(),
            PublicDependencies.ToImmutableArray(),
            Tasks.ToImmutableArray());

        BuildAction?.Invoke(BuiltTarget);

        target = BuiltTarget;
    }

    public IEnumerable<(TargetType?, Target)> GetTargets()
    {
        OutTarget(out var target);
        return [(TargetType, target)];
    }
}
