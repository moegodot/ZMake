using Microsoft.Extensions.Logging;
using StrongInject;
using ZMake.Api;

namespace ZMake;

[Register(typeof(InitializeContextBuilder))]
[Register(typeof(TaskEngine))]
[Register(typeof(BuildPipeline))]
internal partial class Container : IContainer<BuildPipeline>
{
    public Container(
        ILoggerFactory loggerFactory,
        RootPathService service,
        IArtifactsProvider provider)
    {
        ArgumentNullException.ThrowIfNull(loggerFactory);
        ArgumentNullException.ThrowIfNull(service);
        ArgumentNullException.ThrowIfNull(provider);
        Factory = loggerFactory;
        RootPathService = service;
        Provider = provider;
    }

    [Instance] protected IArtifactsProvider Provider { get; }
    [Instance] protected ILoggerFactory Factory { get; }
    [Instance] protected RootPathService RootPathService { get; }
}
