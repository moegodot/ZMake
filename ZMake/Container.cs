using Microsoft.Extensions.Logging;
using StrongInject;
using ZMake.Api;

namespace ZMake;

[Register(typeof(BuildContext))]
[Register(typeof(TaskEngine))]
internal partial class Container : IContainer<BuildContext>
{
    public Container(ILoggerFactory loggerFactory, RootPathService service)
    {
        ArgumentNullException.ThrowIfNull(loggerFactory);
        ArgumentNullException.ThrowIfNull(service);
        Factory = loggerFactory;
        RootPathService = service;
    }

    [Instance] protected ILoggerFactory Factory { get; }
    [Instance] protected RootPathService RootPathService { get; }
}
