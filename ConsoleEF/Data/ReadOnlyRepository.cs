namespace ConsoleEF.Data;

using JetBrains.Annotations;

using Microsoft.EntityFrameworkCore;

using SearchFramework.Paging;
using SearchFramework.SearchCriteria;
using SearchFramework.SortOrder;

using Taazaa.Shared.DevKit.Framework.AsyncLazy;
using Taazaa.Shared.DevKit.Framework.TryHelpers;

[PublicAPI]
public abstract class ReadOnlyRepository<TDbContext, TEntity, TId> : IReadOnlyRepository<TEntity, TId> where TDbContext : DbContext where TEntity : class
{
    private readonly AsyncLazy<TDbContext> lazyDbContext;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ReadOnlyRepository{TDbContext, TEntity, TId}" /> class.
    /// </summary>
    /// <param name="dbContextFactory">
    ///     The database context factory.
    /// </param>
    protected ReadOnlyRepository(IDbContextFactory<TDbContext> dbContextFactory)
    {
        this.lazyDbContext = new AsyncLazy<TDbContext>(() => dbContextFactory.CreateDbContextAsync());
    }

    private TDbContext DbContext => this.lazyDbContext.GetValue();

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
    public Task<Result<TEntity?>> GetByIdAsync(TId id, CancellationToken cancellationToken)
    {
        Task<TEntity?> FindAsync()
        {
            return this.DbContext.Set<TEntity>().FindAsync(new object?[] { id }, cancellationToken).AsTask();
        }

        return TryHelpers.TryAsync((Func<Task<TEntity?>>)FindAsync);
    }

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
    public PagedResult<TEntity?> Search<TSearchCriteria, TSortOrder>(
        TSearchCriteria? searchCriteria = default,
        IEnumerable<TSortOrder>? sortOrder = default,
        PagingInfo? pagingInfo = null,
        CancellationToken cancellationToken = default)
        where TSortOrder : SortOrderBase<TEntity>, new()
        where TSearchCriteria : SearchCriteriaBase<TEntity>, new()
    {
        searchCriteria ??= new TSearchCriteria();
        pagingInfo ??= PagingInfo.Default;

        int skip = pagingInfo.Skip >= 0 ? pagingInfo.Skip : 0;
        int take = int.MaxValue - pagingInfo.Take > 0 ? pagingInfo.Take : 100;

        sortOrder ??= new[] { new TSortOrder() };

        Result<IEnumerable<TEntity>> result = TryHelpers.Try(() => this.DbContext.Set<TEntity>().Where(searchCriteria).ApplySort(sortOrder).Skip(skip)
            .Take(take + 1).AsEnumerable());

        IEnumerable<TEntity> data = result.Map(entities => entities.AsEnumerable(), _ => Enumerable.Empty<TEntity>()).ToArray();

        bool hasMoreData = data.Count() > take;

        return new PagedResult<TEntity?>
        {
            Data = data.Take(take),
            PagingInfo = pagingInfo,
            HasMoreData = hasMoreData,
            Error = result.Map(_ => null!, e => e!),
        };
    }
}