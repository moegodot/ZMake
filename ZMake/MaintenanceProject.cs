using ZMake.Api;
using ZMake.Api.BuiltIn;

namespace ZMake;

public sealed partial class MaintenanceProject(BuildContext buildContext) : ArtifactProject(buildContext)
{
    public override string GroupId { get; } = Api.Version.GroupId;
    public override string ArtifactId { get; } = Api.Version.ArtifactId;
    public override string Version { get; } = Api.Version.Latest.Version.ToString();
    public override PathService Paths { get; } = buildContext.PathService.Create();

    public IEnumerable<TargetBuilder> Targets()
    {
        var toolChainBuilder = ToolChainBuilder.CreateNativeArchitectureEmpty(
            FileFinderBuilder.FromPathExt(),
            new Dictionary<string, string>{
                { "PATH", Context!.PathService.RootDotnetPath },
                { "DOTNET_ROOT", Context!.PathService.RootDotnetPath },
                { "DOTNET_NOLOGO", "true" }
            }
        );

        toolChainBuilder.BinaryFinder.SearchPaths.Add(Context!.PathService.RootDotnetPath);

        // In real environment, call ToolChainBuilder.CreateFromEnvironment().Build();
        var toolChain = toolChainBuilder.Build();

        DotnetTargetEmitter emitter = new(
            Name.Create(ArtifactName, "dotnet"),
            Context!.PathService.RootDotnetPath,
            toolChain);

        yield return emitter
            .AddDotnetTarget(new DotnetArguments("build")
            {
                Configuration = "Release",
            }, "build")
            .Public(TargetTypes.Build)
            .OutName(out var build);

        yield return emitter
            .AddDotnetTarget(new DotnetArguments("run")
            {
                Configuration = "Release",
            }, "run")
            .Public(TargetTypes.Deploy)
            .WithPublicDependencies(build);
    }
}
