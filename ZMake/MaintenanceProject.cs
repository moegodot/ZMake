using ZMake.Api;

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

        var constant =
            FileMatcher.Matches(CurrentSourceDirectory, "*.build.cs")
                .ToBuildConstantBuilderAsSource(context)
                .Build();

        var targetName = Name.Append("dotnet");
        DotnetTargetEmitter emitter = new(
            targetName,
            context.ToolChain);

        yield return new TargetBuilder()
            .WithName(targetName.Append("collect_sources"))
            .Public(TargetType.Builtin.Initialize)
            .WithTasks(context.TaskEngine,(builder) =>
            {

            })
            .OutName(out var collectName);

        yield return emitter
            .AddDotnetTarget([csprojPath],[],new("build")
            {
                Configuration = "Release"
            })
            .DependOn(collectName)
            .Public(TargetType.Builtin.Build)
            .OutName(out var build);

        yield return emitter
            .AddDotnetTarget([csprojPath],[],new("run")
            {
                Configuration = "Release",
            })
            .Public(TargetType.Builtin.Deploy)
            .DependOn(build);
    }
}
