using Microsoft.Extensions.Logging;

namespace ZMake;

public sealed class BuildContext
{
    private readonly ILogger _logger;

    public BuildContext(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger(nameof(BuildContext));

    }


}
