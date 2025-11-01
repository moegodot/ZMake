namespace ZMake.Api;

public interface IContext
{
    RootPathService PathService { get; }

    ToolChain ToolChain { get; }

    TaskEngine TaskEngine { get; }
}
