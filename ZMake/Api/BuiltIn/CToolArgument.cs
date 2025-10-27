namespace ZMake.Api.BuiltIn;

/// <summary>
/// Used in both c and cpp
/// </summary>
public record CToolArgument : ToolArguments
{
    public List<string> Definitions { get; set; }

    public string? LanguageVersion { get; set; } = "c17";

    public OptimizationLevel? Optimization { get; set; }

    /// <summary>
    /// Set false to set `/permissive-` and `/Zc` in msvc.
    /// </summary>
    public bool? Permissive { get; set; } = false;

    public bool? UseUtf8 { get; set; } = true;

    public CToolArgument(bool enableDebug)
    {
        if (enableDebug)
        {
            Optimization = OptimizationLevel.None;
            Definitions = [];
        }
        else
        {
            Optimization = OptimizationLevel.FavourSpeedMedium;
            Definitions = ["NDEBUG"];
        }
    }
}
