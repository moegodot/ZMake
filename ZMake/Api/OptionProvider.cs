using System.Collections.Frozen;

namespace ZMake.Api;

public sealed class OptionProvider
{
    public FrozenDictionary<Name, Option> Options { get; }

    internal OptionProvider(FrozenDictionary<Name, Option> options)
    {
        Options = options;
    }
}
