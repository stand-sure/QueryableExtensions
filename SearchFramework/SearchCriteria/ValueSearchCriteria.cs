#nullable enable

namespace SearchFramework.SearchCriteria;

using System.Text.Json.Serialization;

using JetBrains.Annotations;

using SearchFramework.JsonConverters;
using SearchFramework.TypeSearchExpressions;

[PublicAPI]
public class ValueSearchCriteria<T> : ComparableSearchExpression<T>, IComparableSearchCriteria<T>
{
    private IEnumerable<ValueSearchCriteria<T>>? and;
    private SearchValue<T>? equalTo;
    private SearchValue<T>? greaterThan;
    private SearchValue<T>? greaterThanOrEqualTo;
    private SearchValues<T>? @in;
    private SearchValue<T>? lessThan;
    private SearchValue<T>? lessThanOrEqualTo;
    private SearchValue<T>? notEqualTo;
    private SearchValues<T>? notIn;
    private IEnumerable<ValueSearchCriteria<T>>? or;

    public SearchValue<T>? EqualTo
    {
        get => this.equalTo;
        set
        {
            this.equalTo = value;

            if (this.equalTo is not null)
            {
                this.EqualToSearchExpression = this.equalTo;
            }
        }
    }

    public SearchValue<T>? NotEqualTo
    {
        get => this.notEqualTo;
        set
        {
            this.notEqualTo = value;

            if (this.notEqualTo is not null)
            {
                this.NotEqualToSearchExpression = this.notEqualTo;
            }
        }
    }

    public SearchValue<T>? GreaterThan
    {
        get => this.greaterThan;
        set
        {
            this.greaterThan = value;

            if (this.greaterThan is not null)
            {
                this.GreaterThanSearchExpression = this.greaterThan;
            }
        }
    }

    public SearchValue<T>? GreaterThanOrEqualTo
    {
        get => this.greaterThanOrEqualTo;
        set
        {
            this.greaterThanOrEqualTo = value;

            if (this.greaterThanOrEqualTo is not null)
            {
                this.GreaterThanOrEqualToSearchExpression = this.greaterThanOrEqualTo;
            }
        }
    }

    public SearchValue<T>? LessThan
    {
        get => this.lessThan;
        set
        {
            this.lessThan = value;

            if (this.lessThan is not null)
            {
                this.LessThanSearchExpression = this.lessThan;
            }
        }
    }

    public SearchValue<T>? LessThanOrEqualTo
    {
        get => this.lessThanOrEqualTo;
        set
        {
            this.lessThanOrEqualTo = value;

            if (this.lessThanOrEqualTo is not null)
            {
                this.LessThanOrEqualToSearchExpression = this.lessThanOrEqualTo;
            }
        }
    }

    public IEnumerable<ValueSearchCriteria<T>>? And
    {
        get => this.and;
        set
        {
            this.and = value?.ToList();

            if (this.and?.Any() == true)
            {
                this.AndSearchExpression = this.and.ToArray();
            }
        }
    }

    public SearchValues<T>? In
    {
        get => this.@in;

        set
        {
            this.@in = value;

            if (this.@in is not null)
            {
                this.InSearchExpression = this.@in;
            }
        }
    }

    public SearchValues<T>? NotIn
    {
        get => this.notIn;

        set
        {
            this.notIn = value;

            if (this.notIn is not null)
            {
                this.NotInSearchExpression = this.notIn;
            }
        }
    }

    public IEnumerable<ValueSearchCriteria<T>>? Or
    {
        get => this.or;
        set
        {
            this.or = value?.ToList();

            if (this.or?.Any() == true)
            {
                this.OrSearchExpression = this.or.ToArray();
            }
        }
    }
}