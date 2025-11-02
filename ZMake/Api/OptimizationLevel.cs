namespace ZMake.Api;

public enum OptimizationLevel
{
    /// <summary>
    /// Like O0
    /// </summary>
    None,
    /// <summary>
    /// Like O1
    /// </summary>
    FavourSpeedMinimum,
    /// <summary>
    /// Like O2
    /// </summary>
    FavourSpeedMedium,
    /// <summary>
    /// Like O3
    /// </summary>
    FavourSpeedMaximum,
    /// <summary>
    /// Like Os
    /// </summary>
    FavourSize,
}
