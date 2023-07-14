namespace ConsoleEF;

using ConsoleEF.Data;

using Taazaa.Shared.DevKit.Framework.Search.SearchCriteria;

public class StudentSearchCriteria : SearchCriteriaBase<Student>
{
    public BooleanSearchCriteria? IsEnrolled { get; set; }

    public StringSearchCriteria? Name { get; set; }
    public ValueSearchCriteria<int>? StudentId { get; set; }
}