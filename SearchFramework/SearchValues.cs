namespace ConsoleEF.SearchFramework;

public record SearchValues<TMember>
{
    public IEnumerable<TMember?>? Values { get; init; }
}