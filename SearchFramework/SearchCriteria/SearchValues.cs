namespace ConsoleEF.SearchFramework.SearchCriteria;

public record SearchValues<TMember>
{
    public IEnumerable<TMember?>? Values { get; init; }
}