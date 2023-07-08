#nullable enable

namespace SearchFramework.SortOrder;

using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

public abstract class SortOrderBase : IValidatableObject
{
    protected abstract (string Name, SortOrderDirection Direction) DefaultSort { get; }

    internal (string Name, SortOrderDirection Direction) Sort
    {
        get
        {
            PropertyInfo? propertyInfo = this.GetType().GetProperties()
                .SingleOrDefault(info => info.PropertyType == typeof(SortOrderDirection) && info.GetValue(this) != null);

            string? name = propertyInfo?.Name;
            SortOrderDirection value = (SortOrderDirection?)propertyInfo?.GetValue(this) ?? SortOrderDirection.Ascending;

            return name is null ? (this.DefaultSort.Name, this.DefaultSort.Direction) : (name, value);
        }
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();

        IEnumerable<PropertyInfo> nonNullSortProps =
            this.GetType().GetProperties().Where(info => info.PropertyType == typeof(SortOrderDirection) && info.GetValue(this) != null);

        int? count = nonNullSortProps.Count();

        if (count > 1)
        {
            results.Add(new ValidationResult($"There should only be 0 or 1 SortOrderDirection props with a value. Found {count}"));
        }

        return results;
    }

    public override string ToString()
    {
        return $"{this.Sort.Name} {this.Sort.Direction}";
    }
}