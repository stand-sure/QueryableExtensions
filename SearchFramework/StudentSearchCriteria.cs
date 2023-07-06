namespace ConsoleEF.SearchFramework;

using ConsoleEF.Data;

public class StudentSearchCriteria : SearchCriteriaBase<Student>
{
    public ComparableSearchExpression<int>? StudentId { get; set; }
}