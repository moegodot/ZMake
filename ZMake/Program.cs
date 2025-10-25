using System.Diagnostics;
using System.Reflection;
using ConsoleAppFramework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Semver;
using StrongInject;
using ZLogger;
using ZMake.Api;
using ZMake.Api.BuiltIn;
using static ZMake.LogMessage;

namespace ZMake;

internal static class Program
{
    private static async Task<int> Application(
        string[] args,
        ILoggerFactory loggerFactory)
    {
        try
        {
            var app = ConsoleApp
                .Create()
                .ConfigureServices(collection =>
                {
                    collection.AddSingleton<ILoggerFactory>(loggerFactory);
                });

            app.Add<Commands>();

            await app.RunAsync(args);
        }
        catch (Exception exception)
        {
            exception.PrintColoredStringDemystified();
            return 1;
        }

        return 0;
    }

    public static async Task<int> Main(string[] args)
    {
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.ClearProviders();
            builder.SetMinimumLevel(LogLevel.Trace);
            builder.AddZLoggerConsole();
            builder.AddZLoggerFile("zmake.log.txt");
        });
        Logger = loggerFactory.CreateLogger("zmake");

        Log.AppStart();

        var exitCode = await Application(args, loggerFactory);

        Log.AppStop(exitCode);

        return exitCode;
    }

    private class Commands(ILoggerFactory factory) : IDisposable
    {
        private Owned<BuildContext>? _context;
        private ILoggerFactory LoggerFactory { get; } = factory;

        public BuildContext Context
        {
            get
            {
                if (_context != null) return _context.Value;

                var container = new Container(LoggerFactory);

                _context = container.Resolve();

                Log.BuildContextStart(_context.Value.Id);

                return _context.Value;
            }
        }

        public void Dispose()
        {
            if (_context != null)
            {
                Log.BuildContextStop(_context.Value.Id);
                _context?.Dispose();
            }

            LoggerFactory.Dispose();
            Log.Dispose();
        }

        /// <summary>
        ///     Build targets that has the specified type.
        ///     If type is not an invalid name,it will be parsed as ZMake's built in type.
        ///     For example, `make test` will try parse `test` as a Name of type.
        ///     But apparently it's an invalid Name,so it will be parsed like `moe.kawayi:zmake:1.0.0#target_set:test`.
        ///     For convenience,`make` will be alias to `make build`.
        /// </summary>
        /// <param name="type">The type of targets that will be built.</param>
        public async Task Make(string? type = null)
        {
            Log.CommandStart(nameof(Make));

            type ??= "build";

            var typeName = type switch
            {
                "initialize" => TargetTypes.Initialize,
                "build" => TargetTypes.Build,
                "clean" => TargetTypes.Clean,
                "test" => TargetTypes.Test,
                "package" => TargetTypes.Package,
                "install" => TargetTypes.Install,
                "deploy" => TargetTypes.Deploy,
                _ => TargetType.Parse(type, null)
            };

            await Context.BuildTypedTargets(typeName);

            Log.CommandStop(nameof(Make));
        }
    }
}
