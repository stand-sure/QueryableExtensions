namespace ConsoleEF.SearchFramework.SearchCriteria;

using ConsoleEF.SearchFramework.TypeSearchExpressions;

public class StringSearchCriteria : StringSearchExpression
{
    private readonly SearchValue<string>? contains;
    private readonly SearchValue<string>? endsWith;
    private readonly SearchValue<string>? startsWith;

    public SearchValue<string>? Contains
    {
        get => this.contains;

        init
        {
            this.contains = value;

            if (this.contains is not null)
            {
                this.StringContainsExpression = this.contains;
            }
        }
    }

    public SearchValue<string>? EndsWith
    {
        get => this.endsWith;

        init
        {
            this.endsWith = value;

            if (this.endsWith is not null)
            {
                this.StringEndsWithExpression = this.endsWith;
            }
        }
    }

    public SearchValue<string>? StartsWith
    {
        get => this.startsWith;

        init
        {
            this.startsWith = value;

            if (this.startsWith is not null)
            {
                this.StringStartsWithExpression = this.startsWith;
            }
        }
    }
}