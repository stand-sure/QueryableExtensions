#nullable enable
namespace SearchFramework.Paging;

public class PagedResult<TEntity>
{
    /// <summary>
    ///     Gets or sets the Data.
    /// </summary>
    public IEnumerable<TEntity>? Data { get; set; }

    /// <summary>
    ///     Gets or sets the Error.
    /// </summary>
    public Exception? Error { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether HasMoreData.
    /// </summary>
    public bool HasMoreData { get; set; }

    /// <summary>
    ///     Gets or sets the PagingInfo.
    /// </summary>
    public PagingInfo? PagingInfo { get; set; }
}