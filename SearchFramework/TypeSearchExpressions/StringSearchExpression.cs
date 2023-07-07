#nullable enable

namespace SearchFramework.TypeSearchExpressions;

using SearchFramework.PropertySearchExpressions;
using SearchFramework.SearchCriteria;

using JetBrains.Annotations;

[PublicAPI]
public class StringSearchExpression : ValueSearchCriteria<string>
{
    internal StringContainsSearchExpression? StringContainsExpression { [UsedImplicitly] get; set; }
    internal StringStartsWithSearchExpression? StringStartsWithExpression { [UsedImplicitly] get; set; }
    internal StringEndsWithSearchExpression? StringEndsWithExpression { [UsedImplicitly] get; set; }
}