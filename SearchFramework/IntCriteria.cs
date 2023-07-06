namespace ConsoleEF.SearchFramework;

public class IntCriteria : SearchCriteriaBase<int>
{
    public EqualToSearchExpression<int> EqualToTo => new EqualToSearchExpression<int>();
}