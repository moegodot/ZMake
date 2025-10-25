using Microsoft.Extensions.Logging;
using StrongInject;
using ZMake.Api;

namespace ZMake;

// for user to use
[RegisterModule(typeof(ZMake.Api.Module))]
[Register(typeof(BuildContext))]
[Register(typeof(TaskEngine))]
internal partial class Container : IContainer<BuildContext>
{
    public Container(ILoggerFactory loggerFactory)
    {
        Factory = loggerFactory;
    }

    [Instance] protected ILoggerFactory Factory { get; }
}
