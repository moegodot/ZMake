namespace ZMake.Api;

public static  class ArchitectureExtension
{
    public static Architecture ToArchitecture(this Name name)
    {
        return Architecture.From(name);
    }
}
