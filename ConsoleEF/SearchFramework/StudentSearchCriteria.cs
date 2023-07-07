namespace ConsoleEF.SearchFramework;

using ConsoleEF.Data;
using ConsoleEF.SearchFramework.SearchCriteria;
using ConsoleEF.SearchFramework.TypeSearchExpressions;

public class StudentSearchCriteria : SearchCriteriaBase<Student>
{
    public ComparableSearchExpression<int>? StudentId { get; set; }
    
    public StringSearchExpression? Name { get; set; }
}