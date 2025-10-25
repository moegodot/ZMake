using System.Collections;

namespace ZMake.Api;

public sealed class TargetSet : IEnumerable<ITarget>
{
    private readonly Dictionary<Name, ITarget> _allTargets = [];
    private readonly Dictionary<TargetType, ITarget> _typedTargets = [];

    public IReadOnlyDictionary<Name, ITarget> AllTargets => _allTargets;
    public IReadOnlyDictionary<TargetType, ITarget> TypedTargets => _typedTargets;

    public IEnumerator<ITarget> GetEnumerator()
    {
        return _allTargets.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _allTargets.Values.GetEnumerator();
    }

    public void Add(TargetType? targetType, ITarget target)
    {
        if (_allTargets.ContainsKey(target.Name))
            throw new InvalidOperationException($"The target `{target.Name}` has been in the target set");

        if (targetType is not null) _typedTargets.Add(targetType, target);

        _allTargets.Add(target.Name, target);
    }
}
