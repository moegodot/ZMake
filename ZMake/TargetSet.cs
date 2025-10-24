using System.Collections;
using System.Collections.ObjectModel;

namespace ZMake;

public sealed class TargetSet : IEnumerable<ITarget>
{
    public static readonly Name Build = Name.MakePublicName("target_set", "build");
    public static readonly Name Clean = Name.MakePublicName("target_set", "clean");
    public static readonly Name Test = Name.MakePublicName("target_set", "test");
    public static readonly Name Pack = Name.MakePublicName("target_set", "pack");
    public static readonly Name Install = Name.MakePublicName("target_set", "install");

    private Dictionary<Name, ITarget> _allTargets = [];
    private Dictionary<Name, ITarget> _typedTargets = [];

    public void Add(Name? targetType,ITarget target)
    {
        if (_allTargets.ContainsKey(target.Name))
        {
            throw new InvalidOperationException($"The target `{target.Name}` has been in the target set");
        }

        if (targetType is not null)
        {
            _typedTargets.Add(targetType, target);
        }

        _allTargets.Add(target.Name, target);
    }

    public IReadOnlyDictionary<Name, ITarget> AllTargets => _allTargets;
    public IReadOnlyDictionary<Name, ITarget> TypedTargets => _typedTargets;

    public IEnumerator<ITarget> GetEnumerator()
    {
        return _allTargets.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _allTargets.Values.GetEnumerator();
    }
}
