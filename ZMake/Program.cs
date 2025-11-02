using System.Diagnostics;
using System.Reflection;
using System.Text;
using ConsoleAppFramework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Semver;
using StrongInject;
using ZLogger;
using ZMake.Api;
using static ZMake.LogMessage;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace ZMake;

internal static class Program
{
    public const bool IsBootstrapMode =
#if ZMAKE_BOOTSTRAP_MODE
    true
#else
    false
#endif
    ;

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

#if ZMAKE_BOOTSTRAP_MODE
            app.Add<BootstrapCommands>();
#else
            app.Add<InternalCommands>();
#endif

            await app.RunAsync(args);
        }
        catch (Exception exception)
        {
#if ZMAKE_USE_DEMYSTIFER
            exception.PrintColoredStringDemystified();
#else
            Console.WriteLine(exception.ToString());
#endif
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

    internal class Commands(ILoggerFactory factory) : IDisposable
    {
        private Owned<BuildPipeline> _zmake;
        private BuildContext? _context;

        protected RootPathService? RootPathService = null;

        private ILoggerFactory LoggerFactory { get; } = factory;

        protected BuildContext Context
        {
            get
            {
                if (_context != null) return _context;
                if (RootPathService == null)
                {
                    throw new InvalidOperationException("RootPathService not set");
                }

                var container = new Container(
                    LoggerFactory,
                    RootPathService,
                    new MaintenanceArtifactProvider());

                _zmake = container.Resolve();
                _context = _zmake.Value.Build();

                Log.BuildContextStart(_context.Id);

                return _context;
            }
        }

        protected void SearchRootPath()
        {
            if (IsBootstrapMode)
#pragma warning disable CS0162 // 检测到不可到达的代码
            {
                var directories = Maintenance.SearchSourceAndBuildDirectory();

                if (directories is null)
                {
                    throw new InvalidOperationException(
                        "Failed to find ZMake build directory,check zmake.build.lock file exists");
                }

                RootPathService = new RootPathService(
                    directories.Value.source,
                    directories.Value.build);

                UseDirectory(Logger,
                    RootPathService.RootSourcePath,
                    RootPathService.RootBuildPath);
            }
            else
            {
                throw new InvalidOperationException(
                    "The SearchRootPath() is only available in bootstrap mode");
            }
#pragma warning restore CS0162 // 检测到不可到达的代码
        }

        public void Dispose()
        {
            if (_context != null)
            {
                Log.BuildContextStop(_context.Id);
            }

            _zmake?.Dispose();

            LoggerFactory.Dispose();
            Log.Dispose();
        }
    }

    internal class BootstrapCommands(ILoggerFactory factory) : Commands(factory)
    {
        /// <summary>
        ///     Build targets that has the specified type.
        ///     If type is not an invalid name,it will be parsed as ZMake's built in type.
        ///     For example, `make test` will try parse `test` as a Name of type.
        ///     But apparently it's an invalid Name,so it will be parsed like `moe.kawayi:zmake:1.0.0#target_set:test`.
        ///     For convenience,`make` will be alias to `make build`.
        /// </summary>
        /// <param name="type">The type of targets that will be built.</param>
        public async Task Make(string type = "build")
        {
            Log.CommandStart(nameof(Make));

            SearchRootPath();

            await Context.BuildTypedTargets(TargetType.Builtin.Deploy);

            Log.CommandStop(nameof(Make));
        }

        /// <summary>
        ///     Initialize a repository.
        /// </summary>
        /// <param name="buildDirectory">The build directory of the target repository.</param>
        /// <param name="sourceDirectory">The source directory of the target repository.</param>
        public async Task Initialize(string sourceDirectory, string buildDirectory)
        {
            Log.CommandStart(nameof(Initialize));

            await Maintenance.CreateBuildDirectory(sourceDirectory, buildDirectory);

            Log.CommandStop(nameof(Initialize));
        }
    }

    internal class InternalCommands(ILoggerFactory factory) : Commands(factory)
    {
        public async Task Make(
            string rootSourceDirectory,
            string rootBuildDirectory,
            string type)
        {
            Log.CommandStart(nameof(Make));

            this.RootPathService = new RootPathService
            (
                Path.GetFullPath(rootSourceDirectory),
                Path.GetFullPath(rootBuildDirectory)
            );

            var typeName = type switch
            {
                "initialize" => TargetType.Builtin.Initialize,
                "build" => TargetType.Builtin.Build,
                "clean" => TargetType.Builtin.Clean,
                "test" => TargetType.Builtin.Test,
                "package" => TargetType.Builtin.Package,
                "install" => TargetType.Builtin.Install,
                "deploy" => TargetType.Builtin.Deploy,
                _ => TargetType.Parse(type, null)
            };

            await Context.BuildTypedTargets(typeName);

            Log.CommandStop(nameof(Make));
        }
    }
}
