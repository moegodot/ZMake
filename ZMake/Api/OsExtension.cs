namespace ZMake.Api;

public static class OsExtension
{
    public static Os ToOs(this Name name)
    {
        return Os.From(name);
    }
}
