
namespace ZMake.Api;

public interface ITargetSource
{
    IEnumerable<(TargetType?, Target)> GetTargets();
}
