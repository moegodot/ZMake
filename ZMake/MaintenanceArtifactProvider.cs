
using ZMake.Api;
using ZMake.Api.BuiltIn;

namespace ZMake;

public class MaintenanceArtifactProvider : ArtifactsProviderBase
{
    protected override IEnumerable<IArtifact> GetProjects(InitializeContext context)
    {
        return [new MaintenanceProject()];
    }

    public override void Initialize(InitializeContextBuilder builder)
    {
        var toolChainBuilder = ToolChainBuilder.CreateNativeArchitectureEmpty(
            FileFinderBuilder.FromPathExt(),
            new Dictionary<string, string>{
                { "PATH", builder.PathService.RootDotnetPath },
                { "DOTNET_ROOT", builder.PathService.RootDotnetPath },
                { "DOTNET_NOLOGO", "true" }
            }
        );

        toolChainBuilder.BinaryFinder.SearchPaths.Add(builder.PathService.RootDotnetPath);

        builder.ToolChain = toolChainBuilder;
    }
}
