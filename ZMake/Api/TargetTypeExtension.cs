namespace ZMake.Api;

public static class TargetTypeExtension
{
    public static TargetType ToTargetType(this Name name)
    {
        return TargetType.From(name);
    }
}
