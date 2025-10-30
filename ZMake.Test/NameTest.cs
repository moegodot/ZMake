using ZMake.Api;
using FluentAssertions;
using FluentAssertions.Execution;

namespace ZMake.Test;

public class NameTest
{
    public static readonly ArtifactName ArtifactName = ArtifactName.Create("groupId.test", "artifactId", "1.0.0");

    public static readonly TheoryData<string> NameData = ["test", "test.s_Ub.Upper", "__test__._T1eSt_"];

    public static readonly TheoryData<string> WrongNameData = ["1test", "test.1a", "__test__.1a_", ""];

    [MemberData(nameof(NameData))]
    [Theory]
    public void CreateTest(string nameStr)
    {
        var result = Name.TryCreate(ArtifactName, nameStr, out var name);
        result.Should().BeTrue();
        name?.ArtifactName.Should().BeEquivalentTo(ArtifactName);
        name?.ItemName.Should().BeEquivalentTo(nameStr);
    }

    [MemberData(nameof(NameData))]
    [Theory]
    public void ParseTest(string nameStr)
    {
        var name = Name.Create(ArtifactName, nameStr);

        var clonedName = Name.Parse(name.ToString(), null);

        clonedName.Should().NotBeNull();
        clonedName.Should().BeEquivalentTo(name);
        clonedName.ArtifactName.Should().BeEquivalentTo(ArtifactName);
        clonedName.ItemName.Should().BeEquivalentTo(nameStr);
        clonedName.GetHashCode().Should().Be(name.GetHashCode());
        clonedName.ToString().Should().BeEquivalentTo(name.ToString());
    }

    [Fact]
    public void EqualityTest()
    {
        var name1 = Name.Create(ArtifactName, "test");
        var name2 = Name.Create(ArtifactName, "test");

        name1.Should().BeEquivalentTo(name2);
        name1.GetHashCode().Should().Be(name2.GetHashCode());
        name1.ToString().Should().BeEquivalentTo(name2.ToString());
    }

    [Fact]
    public void DifferenceTest()
    {
        var name1 = Name.Create(ArtifactName, "test1");
        var name2 = Name.Create(ArtifactName, "test2");

        name1.Should().NotBeEquivalentTo(name2);
        name1.GetHashCode().Should().NotBe(name2.GetHashCode());
        name1.ToString().Should().NotBeEquivalentTo(name2.ToString());
    }

    [MemberData(nameof(WrongNameData))]
    [Theory]
    public void WrongFormatTest(string nameStr)
    {
        var result = Name.TryCreate(ArtifactName, nameStr, out var name);
        result.Should().BeFalse();
        name.Should().BeNull();
    }
}
