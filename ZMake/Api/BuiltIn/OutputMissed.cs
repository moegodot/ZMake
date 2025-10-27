namespace ZMake.Api.BuiltIn;

/// <summary>
///
/// </summary>
public sealed class OutputMissed : INotifyBuildChanged
{
    public string[] Source { get; }
    public string[] Output { get; }

    public bool IsChanged {
        get
        {
            foreach (var output in Output)
            {
                if (!File.Exists(output)) return true;
            }
            return false;
        }
    }

    public OutputMissed(string[] source,string[] output)
    {
        Source = source;
        Output = output;
    }
}
