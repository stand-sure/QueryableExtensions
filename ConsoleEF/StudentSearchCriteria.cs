namespace ConsoleEF;

using ConsoleEF.Data;

using SearchFramework.SearchCriteria;
using SearchFramework.TypeSearchExpressions;

public class StudentSearchCriteria : SearchCriteriaBase<Student>
{
    public ComparableSearchExpression<int>? StudentId { get; set; }
    
    public StringSearchExpression? Name { get; set; }
    
    public BooleanSearchCriteria? IsEnrolled { get; set; }
}