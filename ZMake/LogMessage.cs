using Microsoft.Extensions.Logging;

namespace ZMake;

internal partial class LogMessage
{
    [LoggerMessage(EventId = 0, Message = "Could not open socket for {hostName}")]
    internal static partial void CouldNotOpenSocket(ILogger logger, LogLevel level, string hostName);
}
