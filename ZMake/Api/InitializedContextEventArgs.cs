namespace ZMake.Api;

public class InitializedContextEventArgs : EventArgs
{
    public required BuildContext Context { get; init; }
}
