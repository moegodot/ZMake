using Microsoft.Extensions.Logging;
using StrongInject;

namespace ZMake;

// for user to use
[RegisterModule(typeof(ZMake.Module))]

[Register(typeof(Program))]
[Register(typeof(BuildContext))]
[Register(typeof(TaskEngine))]

[RegisterModule(typeof(LoggingModule))]
public partial class Container : IContainer<BuildContext>
{
}
