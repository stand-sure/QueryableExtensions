#nullable enable

namespace SearchFramework.SortOrder;

using System.ComponentModel.DataAnnotations;

public abstract class SortOrderBase<TSource> : IValidatableObject
{
    protected abstract (string Name, ISortOrderDirective Directive) DefaultSort { get; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();

        int? count = this.GetNonNullSortOrderDirectives().Count();

        if (count > 1)
        {
            results.Add(new ValidationResult($"There should only be 0 or 1 SortOrderDirection props with a value. Found {count}"));
        }

        return results;
    }

    private IEnumerable<(string Name, ISortOrderDirective Directive)> GetNonNullSortOrderDirectives()
    {
        IEnumerable<(string Name, ISortOrderDirective Directive)> retVal = from info in this.GetType().GetProperties()
            where typeof(ISortOrderDirective).IsAssignableFrom(info.PropertyType)
            let sortOrderDirective = info.GetValue(this) as ISortOrderDirective
            where sortOrderDirective != null
            select (info.Name, sortOrderDirective);

        return retVal;
    }

    public IOrderedQueryable<TSource> Apply(IQueryable<TSource> queryable)
    {
        string? name;
        ISortOrderDirective? directive;

        List<ValidationResult> validationResults = this.Validate(new ValidationContext(this)).ToList();

        if (validationResults.Any())
        {
            string message = string.Join(null, validationResults.Select(v => v.ErrorMessage));
            throw new ValidationException(message);
        }

        List<(string Name, ISortOrderDirective Directive)> directives = this.GetNonNullSortOrderDirectives().ToList();

        if (directives.Any())
        {
            (name, directive) = directives.Single();
        }
        else
        {
            (name, directive) = this.DefaultSort;
        }

        return directive.Apply(queryable, name);
    }
}