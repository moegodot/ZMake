using System.Collections;
using System.Collections.Frozen;
using System.Collections.Immutable;

namespace ZMake.Api;

public sealed class TargetSetBuilder : IEnumerable<Target>
{
    private bool _built = false;
    private readonly Dictionary<Name, Target> _allTargets = [];
    private readonly Dictionary<TargetType, List<Target>> _typedTargets = [];

    public IReadOnlyDictionary<Name, Target> AllTargets => _allTargets;
    public IReadOnlyDictionary<TargetType, List<Target>> TypedTargets => _typedTargets;

    private void CheckBuilt()
    {
        if (_built)
        {
            throw new InvalidOperationException("The target set has been built");
        }
    }

    public IEnumerator<Target> GetEnumerator()
    {
        CheckBuilt();
        return _allTargets.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        CheckBuilt();
        return _allTargets.Values.GetEnumerator();
    }

    public void Add(TargetType? targetType, Target target)
    {
        CheckBuilt();

        if (_allTargets.ContainsKey(target.Name))
            throw new ArgumentException($"The target `{target.Name}` has been in the target set");

        if (target.AccessibilityLevel != AccessibilityLevel.Public)
        {
            throw new ArgumentException($"The target `{target.Name}` is not public");
        }

        if (targetType is not null)
        {
            if (_typedTargets.TryGetValue(targetType, out var targets))
            {
                targets.Add(target);
            }
            else
            {
                _typedTargets.Add(targetType, [target]);
            }
        }

        _allTargets.Add(target.Name, target);
    }

    public TargetSet Build()
    {
        CheckBuilt();
        _built = true;
        return new TargetSet(
            _allTargets.ToFrozenDictionary(),
            _typedTargets.ToFrozenDictionary(
                static kvp => kvp.Key,
                static kvp => kvp.Value.ToImmutableArray()
            )
        );
    }
}
