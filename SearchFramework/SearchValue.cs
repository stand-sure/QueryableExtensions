namespace ConsoleEF.SearchFramework;

public record SearchValue<TMember>
{
    public TMember? Value { get; init; }

    public static implicit operator SearchValue<TMember>(TMember value)
    {
        return new SearchValue<TMember> { Value = value };
    }
}