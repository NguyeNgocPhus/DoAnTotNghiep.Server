using DoAn.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DoAn.Persistence;

public class UnitOfWorkDbContext<TDbContext> : IUnitOfWorkDbContext<TDbContext> where TDbContext : DbContext
{
    private readonly TDbContext _dbContext;

    public UnitOfWorkDbContext(TDbContext dbContext)
        => _dbContext = dbContext;

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _dbContext.SaveChangesAsync();

    public async ValueTask DisposeAsync()
    {
        await _dbContext.DisposeAsync();
    }
}

