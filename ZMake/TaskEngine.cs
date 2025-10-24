using Microsoft.Extensions.Logging;

namespace ZMake;

public sealed class TaskEngine
{
    private readonly ILogger _logger;

    public TaskEngine(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger(nameof(TaskEngine));


    }

}
