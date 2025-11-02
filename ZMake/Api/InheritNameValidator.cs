using M31.FluentApi.Attributes;

namespace ZMake.Api;

[FluentApi]
public class InheritNameValidator(Name[] parents) : INameValidator
{
    [FluentCollection(0, "Parent")]
    private Name[] Parents { get; init; } = parents;

    public bool Validate(Name other)
    {
        return Parents.Any(other.IsChildOf);
    }
}
