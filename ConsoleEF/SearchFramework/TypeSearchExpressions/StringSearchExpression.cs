namespace ConsoleEF.SearchFramework.TypeSearchExpressions;

using ConsoleEF.SearchFramework.PropertySearchExpressions;
using ConsoleEF.SearchFramework.SearchCriteria;

using JetBrains.Annotations;

public class StringSearchExpression : ValueSearchCriteria<string>
{
    internal StringContainsSearchExpression? StringContainsExpression { [UsedImplicitly] get; set; }
    internal StringStartsWithSearchExpression? StringStartsWithExpression { [UsedImplicitly] get; set; }
    internal StringEndsWithSearchExpression? StringEndsWithExpression { [UsedImplicitly] get; set; }
}