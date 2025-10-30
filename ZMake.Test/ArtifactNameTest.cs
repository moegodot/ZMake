using ZMake.Api;
using FluentAssertions;
using FluentAssertions.Execution;

namespace ZMake.Test;

public class ArtifactNameTest
{
    public static TheoryData<string, string, string> ArtifactData =
        new MatrixTheoryData<string, string, string>(
            ["groupId.test", "groupId.test.s_Ub.Upper", "__groupId__._Test_"],
            ["ArtifactId", "__arTifact_1_.Id", "artifactId.sub_artifact.Upper", "_Upper"],
            ["1.2.3", "1.0.0-alpha", "1.0.0-alpha.1", "1.0.0-rc.1", "1.0.0-beta+exp.sha.5114f85"]
        );

    public static TheoryData<string, string, string> WrongArtifactData =
    new MatrixTheoryData<string, string, string>(
        ["groupId", "1groupId.test", "groupId.1a"],
        ["1artifactId", "1artifactId.sub_artifact", "artifactId.1a"],
        ["1", "1.2", "1.2.3.4", "1.0.0-al_pha"]
    );

    [MemberData(nameof(ArtifactData))]
    [Theory]
    public void CreateTest(string groupId, string artifactId, string version)
    {
        var artifactName =
            ArtifactName.TryCreate(
                groupId,
                artifactId,
                version,
                out var name);

        using (new AssertionScope())
        {
            artifactName.Should().BeTrue();
            name?.GroupId.Should().NotBeNullOrWhiteSpace().And.BeEquivalentTo(groupId);
            name?.ArtifactId.Should().NotBeNullOrWhiteSpace().And.BeEquivalentTo(artifactId);
            name?.Version.ToString().Should().NotBeNullOrWhiteSpace().And.BeEquivalentTo(version);
        }
    }

    [MemberData(nameof(ArtifactData))]
    [Theory]
    public void ParseTest(string groupId, string artifactId, string version)
    {
        var artifactName =
            ArtifactName.Create(
                groupId,
                artifactId,
                version);

        var str = artifactName.ToString();

        var clonedArtifact = ArtifactName.Parse(str, null);

        artifactName.Should().BeEquivalentTo(clonedArtifact);
        artifactName.ToString().Should().NotBeNullOrWhiteSpace().And.BeEquivalentTo(clonedArtifact.ToString());
        artifactName.GetHashCode().Should().Be(clonedArtifact.GetHashCode());
        artifactName.GroupId.Should().NotBeNullOrWhiteSpace().And.BeEquivalentTo(clonedArtifact.GroupId);
        artifactName.ArtifactId.Should().NotBeNullOrWhiteSpace().And.BeEquivalentTo(clonedArtifact.ArtifactId);
        artifactName.Version.ToString().Should().NotBeNullOrWhiteSpace().And.BeEquivalentTo(clonedArtifact.Version.ToString());
        artifactName.Version.Should().BeEquivalentTo(clonedArtifact.Version);
    }

    [Fact]
    public void EqualityTest()
    {
        var artifactName1 = ArtifactName.Create("groupId.test", "artifactId", "1.0.0");
        var artifactName2 = ArtifactName.Create("groupId.test", "artifactId", "1.0.0");

        artifactName1.Should().BeEquivalentTo(artifactName2);
        artifactName1.GetHashCode().Should().Be(artifactName2.GetHashCode());
        artifactName1.ToString().Should().BeEquivalentTo(artifactName2.ToString());
    }

    [Fact]
    public void DifferenceTest()
    {
        var artifactName1 = ArtifactName.Create("groupId.test", "artifactId1", "1.0.0");
        var artifactName2 = ArtifactName.Create("groupId.test", "artifactId2", "1.0.0");
        var artifactName3 = ArtifactName.Create("groupId.test", "artifactId1", "1.1.0");
        var artifactName4 = ArtifactName.Create("groupId.test", "artifactId1", "1.0.0-alpha.0");

        artifactName1.Should().NotBeEquivalentTo(artifactName2);
        artifactName1.GetHashCode().Should().NotBe(artifactName2.GetHashCode());
        artifactName1.ToString().Should().NotBeEquivalentTo(artifactName2.ToString());

        artifactName1.Should().NotBeEquivalentTo(artifactName3);
        artifactName1.GetHashCode().Should().NotBe(artifactName3.GetHashCode());
        artifactName1.ToString().Should().NotBeEquivalentTo(artifactName3.ToString());

        artifactName1.Should().NotBeEquivalentTo(artifactName4);
        artifactName1.GetHashCode().Should().NotBe(artifactName4.GetHashCode());
        artifactName1.ToString().Should().NotBeEquivalentTo(artifactName4.ToString());
    }

    [MemberData(nameof(WrongArtifactData))]
    [Theory]
    public void WrongFormatTest(string groupId, string artifactId, string version)
    {
        var result =
            ArtifactName.TryCreate(
                groupId,
                artifactId,
                version,
                out var artifactName);

        result.Should().BeFalse();
        artifactName.Should().BeNull();
    }
}
