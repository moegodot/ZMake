using System.Diagnostics.Tracing;
using Microsoft.Extensions.Logging;
using ZLogger;
using ZMake.Api;

namespace ZMake;

[EventSource(Name = "MoeDotKawayi-ZMake")]
internal sealed partial class LogMessage : EventSource
{
    public static readonly LogMessage Log = new();

    public static ILogger Logger { get; set; } = null!;

    [Event(1, Keywords = Keywords.Application, Task = Tasks.Application, Opcode = EventOpcode.Start)]
    public void AppStart()
    {
        WriteEvent(1);
    }

    [Event(2, Keywords = Keywords.Application, Task = Tasks.Application, Opcode = EventOpcode.Stop)]
    public void AppStop(int exitCode)
    {
        WriteEvent(2, exitCode);
    }

    [Event(3, Keywords = Keywords.Command, Task = Tasks.Command, Opcode = EventOpcode.Start)]
    public void CommandStart(string command)
    {
        WriteEvent(3, command);
    }

    [Event(4, Keywords = Keywords.Command, Task = Tasks.Command, Opcode = EventOpcode.Stop)]
    public void CommandStop(string command)
    {
        WriteEvent(4, command);
    }

    [Event(5, Keywords = Keywords.Target, Task = Tasks.Target, Opcode = EventOpcode.Start)]
    public void TargetStart(string name)
    {
        TargetStart(Logger, name);
        WriteEvent(5, name);
    }

    [Event(6, Keywords = Keywords.Target, Task = Tasks.Target, Opcode = EventOpcode.Stop)]
    public void TargetStop(string name, Exception? exception)
    {
        TargetStop(Logger, exception, name);
        WriteEvent(6, name);
    }

    [Event(7, Keywords = Keywords.BuildContext, Task = Tasks.BuildContext, Opcode = EventOpcode.Start)]
    public void BuildContextStart(long id)
    {
        WriteEvent(7, id);
    }

    [Event(8, Keywords = Keywords.BuildContext, Task = Tasks.BuildContext, Opcode = EventOpcode.Stop)]
    public void BuildContextStop(long id)
    {
        WriteEvent(8, id);
    }

    [NonEvent]
    [ZLoggerMessage(EventId = 5, Level = LogLevel.Debug, Message = "Target {targetName} started")]
    internal static partial void TargetStart(ILogger logger, string targetName);

    [NonEvent]
    [ZLoggerMessage(EventId = 6, Level = LogLevel.Debug, Message = "Target {targetName} was finished")]
    internal static partial void TargetStop(ILogger logger, Exception? exception, string targetName);

    [NonEvent]
    [ZLoggerMessage(Level = LogLevel.Information, Message = "Detect system information {item} = {value}")]
    internal static partial void DetectSystemInformation(ILogger logger,string item,string value);

    [NonEvent]
    [ZLoggerMessage(Level = LogLevel.Information, Message = "Use source dir `{source}`,build dir `{build}`")]
    internal static partial void UseDirectory(ILogger logger,string source,string build);

    public static class Keywords // This is a bitvector
    {
        public const EventKeywords Application = (EventKeywords)0x0001;
        public const EventKeywords Command = (EventKeywords)0x0002;
        public const EventKeywords Target = (EventKeywords)0x0004;
        public const EventKeywords BuildContext = (EventKeywords)0x0008;
    }

    public class Tasks
    {
        public const EventTask Application = (EventTask)0x0001;
        public const EventTask Command = (EventTask)0x0002;
        public const EventTask Target = (EventTask)0x0004;
        public const EventTask BuildContext = (EventTask)0x0008;
    }
}
