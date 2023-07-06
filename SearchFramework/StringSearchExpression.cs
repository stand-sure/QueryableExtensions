namespace ConsoleEF.SearchFramework;

using JetBrains.Annotations;

public class StringSearchExpression : ComparableSearchExpression<string>
{
    internal StringContainsExpression? StringContainsExpression { [UsedImplicitly] get; set; }
}