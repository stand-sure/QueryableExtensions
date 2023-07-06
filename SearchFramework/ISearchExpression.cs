namespace ConsoleEF.SearchFramework;

using System.Linq.Expressions;

public interface ISearchExpression
{
    public Expression GetExpression(MemberExpression memberExpression);
}