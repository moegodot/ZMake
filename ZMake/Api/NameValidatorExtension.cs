namespace ZMake.Api;

public static class NameValidatorExtension
{
    public static InheritNameValidator CreateInheritValidator(this Name name)
    {
        return CreateInheritNameValidator.WithParent(name);
    }

    public static AnyListNameValidator CreateAnyWith(this INameValidator nameValidator, params INameValidator[] others)
    {
        return CreateAnyListNameValidator.WithValidators([nameValidator, ..others]);
    }
}
