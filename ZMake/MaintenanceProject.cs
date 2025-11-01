using ZMake.Api;
using ZMake.Api.BuiltIn;

namespace ZMake;

[ArtifactProject]
public sealed partial class MaintenanceProject
{
    public static ArtifactName GetArtifactName()
    {
        return ArtifactName.CreateWithNewArtifactId(Api.Version.Latest, "zmake.maintenance");
    }

    public const string CsProjectFileName = "ZMakefile.csproj";

    public IEnumerable<ITargetSource> GetTargets(InitializeContext context)
    {
        var csprojPath = $"{context.PathService.RootDotnetPath}/{CsProjectFileName}";

        DotnetTargetEmitter emitter = new(
            Name.Append("dotnet"),
            context.ToolChain);

        yield return new TargetBuilder();

        yield return emitter
            .AddDotnetTarget([csprojPath],[],new("build")
            {
                Configuration = "Release"
            })
            .Public(TargetTypes.Build)
            .OutName(out var build);

        yield return emitter
            .AddDotnetTarget([csprojPath],[],new("run")
            {
                Configuration = "Release",
            })
            .Public(TargetTypes.Deploy)
            .WithPublicDependencies(build);
    }
}
