using System.Collections.Frozen;
using System.Collections.Immutable;

namespace ZMake.Api;

public record class BuildEnvironment(
    FrozenDictionary<string,string> EnvironmentVariables,
    string WorkDirectory)
{
}
