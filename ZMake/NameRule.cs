using System.Text.RegularExpressions;

namespace ZMake;

internal static partial class NameRule
{
    private const string IdRegexBase = "^[a-zA-Z_]+[a-zA-Z0-9_]*(\\.[a-zA-Z_]+[a-zA-Z0-9_]*)";
    private const string MustHaveMultipleParts = $"{IdRegexBase}+$";
    private const string MaybeSinglePart = $"{IdRegexBase}*$";

    [GeneratedRegex(MustHaveMultipleParts)]
    public static partial Regex MustHaveMultiplePartsRegex();

    [GeneratedRegex(MaybeSinglePart)]
    public static partial Regex MaybeSinglePartRegex();
}
