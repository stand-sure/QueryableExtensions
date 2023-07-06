namespace ConsoleEF.SearchFramework;

using JetBrains.Annotations;

public class StringSearchExpression : ValueSearchCriteria<string>
{
    internal StringContainsExpression? StringContainsExpression { [UsedImplicitly] get; set; }
    internal StringStartsWithExpression? StringStartsWithExpression { [UsedImplicitly] get; set; }
    internal StringEndsWithExpression? StringEndsWithExpression { [UsedImplicitly] get; set; }
}