using System.Collections;
using System.Collections.Frozen;
using System.Collections.Immutable;

namespace ZMake.Api;

public sealed class TargetSet : IEnumerable<Target>
{
    private readonly FrozenDictionary<Name, Target> _targets;
    private readonly FrozenDictionary<TargetType, ImmutableArray<Target>> _typedTargets;

    internal TargetSet(
        FrozenDictionary<Name, Target> targets,
        FrozenDictionary<TargetType, ImmutableArray<Target>> typedTargets)
    {
        _targets = targets;
        _typedTargets = typedTargets;
    }

    public IReadOnlyDictionary<Name, Target> Targets => _targets;
    public IReadOnlyDictionary<TargetType, ImmutableArray<Target>> TypedTargets => _typedTargets;

    public Target? this[Name name] => _targets.GetValueRefOrNullRef(name);
    public ImmutableArray<Target>? this[TargetType targetType] => _typedTargets.GetValueRefOrNullRef(targetType);

    public IEnumerator<Target> GetEnumerator()
    {
        var it = _targets.Values.GetEnumerator();
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
