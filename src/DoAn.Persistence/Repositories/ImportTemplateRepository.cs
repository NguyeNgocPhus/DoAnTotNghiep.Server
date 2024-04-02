using DoAn.Application.Abstractions.Repositories;
using DoAn.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DoAn.Persistence.Repositories;

public class ImportTemplateRepository : RepositoryBase<ImportTemplate, Guid>, IImportTemplateRepository
{
    private readonly ApplicationDbContext _dbContext;
    public ImportTemplateRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<ImportTemplate?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _dbContext.ImportTemplates.FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
    }
}