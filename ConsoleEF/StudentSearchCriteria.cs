namespace ConsoleEF;

using ConsoleEF.Data;

using SearchFramework.SearchCriteria;
using SearchFramework.TypeSearchExpressions;

public class StudentSearchCriteria : SearchCriteriaBase<Student>
{
    public BooleanSearchCriteria? IsEnrolled { get; set; }

    public StringSearchCriteria? Name { get; set; }
    public ComparableSearchExpression<int>? StudentId { get; set; }
}