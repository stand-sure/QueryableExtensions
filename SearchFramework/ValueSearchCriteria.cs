namespace ConsoleEF.SearchFramework;

public class ValueSearchCriteria<T> : ComparableSearchExpression<T>, IComparableSearchCriteria<T>
{
    private readonly SearchValue<T>? equalTo;
    private readonly SearchValue<T>? greaterThan;
    private readonly SearchValue<T>? greaterThanOrEqualTo;
    private readonly SearchValue<T>? lessThan;
    private readonly SearchValue<T>? lessThanOrEqualTo;
    private readonly SearchValue<T>? notEqualTo;
    private readonly SearchValues<T>? @in;

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
}