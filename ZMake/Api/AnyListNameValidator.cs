
using M31.FluentApi.Attributes;

namespace ZMake.Api;

[FluentApi]
public class AnyListNameValidator(INameValidator[] validators) : INameValidator
{
    [FluentCollection(0, "Validators")]
    private INameValidator[] Validators { get; init; } = validators;

    public bool Validate(Name other)
    {
        return Validators.Any(validator => validator.Validate(other));
    }
}
