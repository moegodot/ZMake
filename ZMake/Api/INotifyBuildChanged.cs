using JetBrains.Annotations;

namespace ZMake.Api;

[PublicAPI]
public interface INotifyBuildChanged
{
    string[] Source { get; }
    string[] Output { get; }
    bool IsChanged { get; }
}
