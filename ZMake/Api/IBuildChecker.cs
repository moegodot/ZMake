using JetBrains.Annotations;

namespace ZMake.Api;

[PublicAPI]
public interface IBuildChecker
{
    Task Update(BuildConstant build);
    Task<bool> CheckChanged(BuildConstant build);
}
