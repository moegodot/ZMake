namespace ZMake.Api;

public static class NameExtension
{
    public static Name Append(this ArtifactName artifactName, string str)
    {
        return Name.Create(artifactName, str);
    }
}
