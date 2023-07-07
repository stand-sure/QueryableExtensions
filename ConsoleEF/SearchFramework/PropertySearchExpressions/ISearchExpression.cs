namespace ConsoleEF.SearchFramework.PropertySearchExpressions;

using System.Linq.Expressions;

public interface ISearchExpression
{
    public Expression GetExpression(MemberExpression memberExpression);
}