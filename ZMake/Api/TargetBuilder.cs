using System.Collections.Immutable;
using System.Diagnostics.Contracts;

namespace ZMake.Api.BuiltIn;

public sealed class TargetBuilder
{
    public Name? TargetName { get; set; } = null;

    public TargetType? TargetType { get; set; } = null;

    public AccessibilityLevel AccessibilityLevel { get; set; } = AccessibilityLevel.Private;

    public List<Target> PrivateDependencies { get; } = [];

    public List<Name> PublicDependencies { get; } = [];

    public List<Task<Task>> Tasks { get; } = [];

    public Action<Target>? BuildAction { get; set; } = null;

    public TargetBuilder WithName(Name targetName)
    {
        TargetName = targetName;
        return this;
    }

    public TargetBuilder WithPrivateDependencies(IEnumerable<Target> privateDependencies)
    {
        PrivateDependencies.AddRange(privateDependencies);
        return this;
    }

    public TargetBuilder WithPrivateDependencies(Target privateDependencies)
    {
        PrivateDependencies.Add(privateDependencies);
        return this;
    }

    public TargetBuilder WithPublicDependencies(IEnumerable<Name> publicDependencies)
    {
        PublicDependencies.AddRange(publicDependencies);
        return this;
    }

    public TargetBuilder WithPublicDependencies(Name publicDependencies)
    {
        PublicDependencies.Add(publicDependencies);
        return this;
    }

    public TargetBuilder WithTasks(IEnumerable<Task<Task>> tasks)
    {
        Tasks.AddRange(tasks);
        return this;
    }

    public TargetBuilder Public(TargetType? targetType)
    {
        TargetType = targetType;
        AccessibilityLevel = AccessibilityLevel.Public;
        return this;
    }

    public TargetBuilder PrivateAndClearType()
    {
        TargetType = null;
        AccessibilityLevel = AccessibilityLevel.Private;
        return this;
    }

    public TargetBuilder WithTask(Task<Task> task)
    {
        Tasks.Add(task);
        return this;
    }

    public TargetBuilder WithTask(Task task)
    {
        Tasks.Add(new Task<Task>(static async (object? o) => await (Task)o!, task));
        return this;
    }

    public TargetBuilder WithToolCall(ITool tool,
    IEnumerable<string> arguments,
    string? workDir = null,
    IReadOnlyDictionary<string, string>? environment = null)
    {
        Tasks.Add(new Task<Task>(async () => await tool.Call(arguments, workDir, environment)));
        return this;
    }

    public TargetBuilder WithToolCall<T>(ITool<T> tool, T arguments, string? workDir = null, IReadOnlyDictionary<string, string>? environment = null) where T : ToolArguments
    {
        Tasks.Add(new Task<Task>(async () => await tool.Call(arguments, workDir, environment)));
        return this;
    }

    public TargetBuilder WithBuildToolCall<T>(IBuildTool<T> tool, IEnumerable<string> @in, IEnumerable<string> @out, T arguments, string? workDir = null, IReadOnlyDictionary<string, string>? environment = null) where T : ToolArguments
    {
        Tasks.Add(new Task<Task>(async () => await tool.Build(@in, @out, arguments, workDir, environment)));
        return this;
    }

    public TargetBuilder WithBuildToolCall<T>(ToolChain toolChain, ToolType toolType, IEnumerable<string> @in, IEnumerable<string> @out, T arguments) where T : ToolArguments
    {
        Tasks.Add(new Task<Task>(async () => await ((IBuildTool<T>?)toolChain[toolType] ?? throw new InvalidOperationException($"Failed to find the program `{toolType}`")).Build(@in, @out, arguments, null, toolChain.Environments)));
        return this;
    }

    public TargetBuilder WithBuildAction(Action<Target> action)
    {
        BuildAction = action;
        return this;
    }

    public TargetBuilder WithBuildActionWriteTo(Dictionary<Name, Target> targets)
    {
        return WithBuildAction((target) => targets.Add(target.Name, target));
    }

    public TargetBuilder OutName(out Name name)
    {
        name = TargetName ?? throw new InvalidOperationException("Target name is not set");
        return this;
    }

    public Target Build()
    {
        if (AccessibilityLevel == AccessibilityLevel.Private
            && TargetType is not null)
        {
            throw new InvalidOperationException("When accessibility level is private,target type must be null");
        }

        var target = new Target(
            TargetName ?? throw new InvalidOperationException("Target name is not set"),
            AccessibilityLevel,
            PrivateDependencies.ToImmutableArray(),
            PublicDependencies.ToImmutableArray(),
            Tasks.ToImmutableArray());

        BuildAction?.Invoke(target);

        return target;
    }
}
