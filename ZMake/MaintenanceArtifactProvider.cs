
using ZMake.Api;
using ZMake.Api.BuiltIn;

namespace ZMake;

public class MaintenanceArtifactProvider : ArtifactsProviderBase
{

    public static readonly ArtifactName ArtifactName =
        ArtifactName.CreateWithNewArtifactId(Api.Version.Latest, "zmake.maintain");

    public TargetSet Targets { get; } = null!;

    public ITool? Dotnet { get; set; }

    protected override void AfterBind()
    {

    }

    public override IEnumerable<Artifact> Get()
    {
        yield return new Artifact(ArtifactName, Targets);
    }
}
