using System.Diagnostics;
using ZMake.Api;

namespace ZMake.Api;

public sealed class Option
{
    public Name Key { get; }

    public object Value { get; }

    public OptionType Type { get; }

    public Option(Name key, object value, OptionType type)
    {
        Key = key;

        if (!(type switch
        {
            OptionType.String => value is string,
            OptionType.Number => value is long,
            OptionType.Float => value is double,
            OptionType.Boolean => value is bool,
            _ => throw new UnreachableException()
        }))
        {
            throw new ArgumentException("The value type is not match the option type");
        }

        Value = value;
        Type = type;
    }

    public Option(Name key, object value)
    {
        Key = key;

        if (value is string)
        {
            Type = OptionType.String;
        }
        else if (value is long)
        {
            Type = OptionType.Number;
        }
        else if (value is double)
        {
            Type = OptionType.Float;
        }
        else if (value is bool)
        {
            Type = OptionType.Boolean;
        }
        else
        {
            throw new ArgumentException("The value type is not match the option type");
        }

        Value = value;
    }

    public string AsString()
    {
        if (Type != OptionType.String)
        {
            throw new InvalidOperationException("The option is not a string");
        }

        return (string)Value;
    }

    public long AsNumber()
    {
        if (Type != OptionType.Number)
        {
            throw new InvalidOperationException("The option is not a number");
        }

        return (long)Value;
    }

    public double AsFloat()
    {
        if (Type != OptionType.Float)
        {
            throw new InvalidOperationException("The option is not a float");
        }

        return (double)Value;
    }

    public bool AsBoolean()
    {
        if (Type != OptionType.Boolean)
        {
            throw new InvalidOperationException("The option is not a boolean");
        }

        return (bool)Value;
    }

    public override string ToString()
    {
        return $"{Key}: {Value} ({Type})";
    }

    public override bool Equals(object? obj)
    {
        return obj is Option option &&
               Key.Equals(option.Key) &&
               Value.Equals(option.Value) &&
               Type.Equals(option.Type);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Key, Value, Type);
    }
}
