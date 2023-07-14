namespace ConsoleEF.Data;

using SearchFramework.Paging;
using SearchFramework.SearchCriteria;
using SearchFramework.SortOrder;

using Taazaa.Shared.DevKit.Framework.TryHelpers;

public interface IReadOnlyRepository<TEntity, in TId> where TEntity : class
{
    /// <summary>
    ///     Gets the by identifier.
    /// </summary>
    /// <param name="id">
    ///     The identifier.
    /// </param>
    /// <param name="cancellationToken">
    ///     The cancellation token.
    /// </param>
    /// <returns>
    ///     The <see cref="Task" />.
    /// </returns>
    Task<Result<TEntity?>> GetByIdAsync(TId id, CancellationToken cancellationToken);

    /// <summary>
    ///     Searches using the specified search criteria.
    /// </summary>
    /// <param name="searchCriteria">
    ///     The search criteria.
    /// </param>
    /// <param name="sortOrder">
    ///     The sort order.
    /// </param>
    /// <param name="pagingInfo">
    ///     The paging info.
    /// </param>
    /// <param name="cancellationToken">
    ///     The cancellation token.
    /// </param>
    /// <typeparam name="TSearchCriteria">
    ///     The type of the search criteria.
    /// </typeparam>
    /// <typeparam name="TSortOrder">
    ///     The type of the sort order.
    /// </typeparam>
    /// <returns>
    ///     The <see cref="PagedResult{TEntity}" />.
    /// </returns>
    PagedResult<TEntity?> Search<TSearchCriteria, TSortOrder>(
        TSearchCriteria? searchCriteria = default,
        IEnumerable<TSortOrder>? sortOrder = default,
        PagingInfo? pagingInfo = null,
        CancellationToken cancellationToken = default)
        where TSortOrder : SortOrderBase<TEntity>, new()
        where TSearchCriteria : SearchCriteriaBase<TEntity>, new();
}