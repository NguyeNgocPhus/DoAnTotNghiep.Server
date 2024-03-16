namespace DoAn.Application.Abstractions;

public interface IUnitOfWorkDbContext<TDbContext>: IAsyncDisposable
{
    /// <summary>
    /// Call save change from db context
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}