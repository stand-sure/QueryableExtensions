#nullable enable

namespace SearchFramework.SearchCriteria;

using JetBrains.Annotations;

using SearchFramework.TypeSearchExpressions;

[PublicAPI]
public class ValueSearchCriteria<T> : ComparableSearchExpression<T>, IComparableSearchCriteria<T>
{
    private readonly IEnumerable<ValueSearchCriteria<T>>? and;
    private readonly SearchValue<T>? equalTo;
    private readonly SearchValue<T>? greaterThan;
    private readonly SearchValue<T>? greaterThanOrEqualTo;
    private readonly SearchValues<T>? @in;
    private readonly SearchValue<T>? lessThan;
    private readonly SearchValue<T>? lessThanOrEqualTo;
    private readonly SearchValue<T>? notEqualTo;
    private readonly SearchValues<T>? notIn;
    private readonly IEnumerable<ValueSearchCriteria<T>>? or;

    public SearchValue<T>? EqualTo
    {
        get => this.equalTo;
        init
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
        init
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
        init
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
        init
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
        init
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
        init
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
        init
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

        init
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

        init
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
        init
        {
            this.or = value?.ToList();

            if (this.or?.Any() == true)
            {
                this.OrSearchExpression = this.or.ToArray();
            }
        }
    }
}