namespace ConsoleEF.Data;

using Microsoft.EntityFrameworkCore;

using SearchFramework.Paging;
using SearchFramework.SearchCriteria;
using SearchFramework.SortOrder;

using Taazaa.Shared.DevKit.Framework.AsyncLazy;
using Taazaa.Shared.DevKit.Framework.TryHelpers;

public abstract class ReadWriteRepository<TDbContext, TEntity, TCreateInfo, TUpdateInfo, TId> : IReadOnlyRepository<TEntity, TId> where TEntity : class where TDbContext : DbContext
{
    private readonly AsyncLazy<TDbContext> lazyDbContext;

    private TDbContext DbContext => this.lazyDbContext.GetValue();
    private ReadOnlyRepository<TDbContext, TEntity, TId> ReadOnlyRepository { get; }

    protected ReadWriteRepository(IDbContextFactory<TDbContext> dbContextFactory, ReadOnlyRepository<TDbContext, TEntity, TId> readOnlyRepository)
    {
        this.ReadOnlyRepository = readOnlyRepository;
        this.lazyDbContext = new AsyncLazy<TDbContext>(() => dbContextFactory.CreateDbContextAsync());
    }

    protected abstract Func<TCreateInfo, TEntity> DefaultCreateEntityFunction { get; }

    public async Task<Result<TEntity>> CreateAsync(TCreateInfo createInfo, Func<TCreateInfo, TEntity>? creator = null, CancellationToken cancellationToken = default)
    {
        creator ??= this.DefaultCreateEntityFunction;
        TEntity entity = creator(createInfo);
        this.DbContext.Set<TEntity>().Add(entity);
        Result<int> saveResult = await TryHelpers.TryAsync(() => this.DbContext.SaveChangesAsync(cancellationToken));

        Result<TEntity> result = saveResult.HasFailed ? saveResult.Map(_ => null!, e => e!) : entity;

        return result;
    }

    protected abstract Func<TEntity?, TUpdateInfo?, TEntity?> DefaultUpdateEntityFunction { get; }

    public async Task<Result<TEntity?>> UpdateAsync(
        TId id,
        TUpdateInfo updateInfo,
        CancellationToken cancellationToken = default)
    {
        Func<TEntity?, TUpdateInfo?, TEntity?>? updater = this.DefaultUpdateEntityFunction;

        Result<TEntity?> e = await this.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        async Task<Result<TEntity?>> OnSuccessfulGet(TEntity? entity)
        {
            Result<TEntity?> result;

            entity = updater(entity, updateInfo);
            
            if (entity is null)
            {
                result = (TEntity?)null;
                return result;
            }
            
            this.DbContext.Set<TEntity>().Add(entity);
            Result<int> saveResult = await TryHelpers.TryAsync(() => this.DbContext.SaveChangesAsync(cancellationToken));

            result = saveResult.HasFailed ? saveResult.Map(_ => null!, exception => exception!) : entity;

            return result;
        }

        static Task<Result<TEntity?>> OnFailedGet(Exception ex)
        {
            Result<TEntity?> r = ex;
            return Task.FromResult(r);
        }

        return await e.Map(OnSuccessfulGet, OnFailedGet).ConfigureAwait(false);
    }
    
    public async Task<Result<TEntity?>> UpdateAsync(
        TId id,
        Func<TEntity?, TEntity?> updater,
        CancellationToken cancellationToken = default)
    {
        Result<TEntity?> e = await this.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        async Task<Result<TEntity?>> OnSuccessfulGet(TEntity? entity)
        {
            Result<TEntity?> result;

            entity = updater(entity);
            
            if (entity is null)
            {
                result = (TEntity?)null;
                return result;
            }
            
            this.DbContext.Set<TEntity>().Add(entity);
            Result<int> saveResult = await TryHelpers.TryAsync(() => this.DbContext.SaveChangesAsync(cancellationToken));

            result = saveResult.HasFailed ? saveResult.Map(_ => null!, exception => exception!) : entity;

            return result;
        }

        static Task<Result<TEntity?>> OnFailedGet(Exception ex)
        {
            Result<TEntity?> r = ex;
            return Task.FromResult(r);
        }

        return await e.Map(OnSuccessfulGet, OnFailedGet).ConfigureAwait(false);
    }

    public Task<Result<TEntity?>> GetByIdAsync(TId id, CancellationToken cancellationToken)
    {
        return this.ReadOnlyRepository.GetByIdAsync(id, cancellationToken);
    }

    public PagedResult<TEntity?> Search<TSearchCriteria, TSortOrder>(
        TSearchCriteria? searchCriteria = default,
        IEnumerable<TSortOrder>? sortOrder = default,
        PagingInfo? pagingInfo = null,
        CancellationToken cancellationToken = default) where TSearchCriteria : SearchCriteriaBase<TEntity>, new()
        where TSortOrder : SortOrderBase<TEntity>, new()
    {
        return this.ReadOnlyRepository.Search(searchCriteria, sortOrder, pagingInfo, cancellationToken);
    }
}