namespace ConsoleEF.SearchFramework;

public record SearchValue<TMember>
{
    public TMember? Value { get; init; }
}