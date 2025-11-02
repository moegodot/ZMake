namespace ZMake.Api;

/// <summary>
/// Used in both c and cpp
/// </summary>
public class CToolArgument : ToolArguments
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

    public override UInt128 GetHashCode128()
    {
        return HashCode128.Combine(
            HashCode128.Get(nameof(CToolArgument)),
            Definitions.GetEnumerableHashCode128(),
            HashCode128.Get(LanguageVersion ?? string.Empty),
            HashCode128.Get(Optimization?.ToString() ?? string.Empty),
            HashCode128.Get(Permissive?.ToString() ?? string.Empty),
            HashCode128.Get(UseUtf8?.ToString() ?? string.Empty),
            base.GetHashCode128());
    }
}
