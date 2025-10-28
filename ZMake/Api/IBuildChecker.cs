using JetBrains.Annotations;

namespace ZMake.Api;

[PublicAPI]
public interface IBuildChecker<T> where T : ToolArguments
{
    Task Update(BuildConstant<T> build);
    Task<bool> CheckChanged(BuildConstant<T> build);
}
