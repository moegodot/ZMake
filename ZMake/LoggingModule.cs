using Microsoft.Extensions.Logging;
using StrongInject;

namespace ZMake;

public class LoggingModule : IFactory<ILogger>
{
    [Instance]
    protected ILoggerFactory Factory { get; }

    public LoggingModule(ILoggerFactory factory) => Factory = factory;

    public ILogger Create()
    {
        return Factory.CreateLogger("ZMake");
    }
}
