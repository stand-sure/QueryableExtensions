namespace Service;

using System.Text.RegularExpressions;

public class QueryMutationControllerParameterTransformer : IOutboundParameterTransformer
{
    /// <inheritdoc />
    public string? TransformOutbound(object? value)
    {
        if (value is null)
        {
            return null;
        }

        string newValue = Regex.Replace(
            value.ToString()!,
            "(Mutation|Query)",
            string.Empty,
            RegexOptions.CultureInvariant,
            TimeSpan.FromMilliseconds(100)
        );

        return newValue;
    }
}