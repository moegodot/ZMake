namespace ZMake.Api;

public interface IBuilder<T>
{


    IEnumerable<IBuildTool<T>> BuildTools { get; }

}
