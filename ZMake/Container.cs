using Microsoft.Extensions.Logging;
using StrongInject;

namespace ZMake;

// for user to use
[RegisterModule(typeof(ZMake.Module))]

[Register(typeof(Program))]
[Register(typeof(BuildContext))]
[Register(typeof(TaskEngine))]
public partial class Container : IContainer<BuildContext>
{
    [Instance]
    protected ILoggerFactory Factory { get; }

    public Container(ILoggerFactory factory) => Factory = factory;
}
