using System.Collections;
using System.Collections.Frozen;
using System.Collections.Immutable;

namespace ZMake.Api;

public sealed class TargetSet : IEnumerable<Target>
{
    internal TargetSet(
        FrozenDictionary<Name, Target> targets,
        FrozenDictionary<TargetType, ImmutableArray<Target>> typedTargets)
    {
        Targets = targets;
        TypedTargets = typedTargets;
    }

    public FrozenDictionary<Name, Target> Targets { get; }
    public FrozenDictionary<TargetType, ImmutableArray<Target>> TypedTargets { get; }

    public Target? this[Name name] => Targets.GetValueRefOrNullRef(name);

    public ImmutableArray<Target>? this[TargetType targetType] => TypedTargets.GetValueRefOrNullRef(targetType);

    public IEnumerator<Target> GetEnumerator()
    {
        var it = Targets.Values.GetEnumerator();
        while (it.MoveNext())
        {
            yield return it.Current;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
