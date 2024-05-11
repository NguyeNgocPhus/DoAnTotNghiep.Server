using DoAn.Application.Abstractions.Repositories;
using DoAn.Domain.Entities;
using DoAn.Shared.Services.V1.ImportHistory.Responses;
using DoAn.Shared.Services.V1.ImportTemplate.Responses;
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

    public IQueryable<ImportTemplateResponse> GetImportTemplates(CancellationToken cancellationToken = default)
    {
        var query = from it in _dbContext.ImportTemplates
            join f in _dbContext.FileStorages on it.FileTemplateId equals f.Id
            select new ImportTemplateResponse()
            {
                Id = it.Id,
                Name = it.Name,
                Description = it.Description,
                Active = it.Active,
                CreatedTime = it.CreatedTime,
                HasWorkflow = it.HasWorkflow,
                IsDeleted = it.IsDeleted,
                Tag = it.Tag,
                FileTemplateId = f.Id,
                FileTemplateName = f.OriginalName,

            };
       
        return query;

    }

    public async Task<List<ImportTemplate>> FindIgnoreDelete(CancellationToken cancellationToken = default)
    {
        return await _dbContext.ImportTemplates.Where(x => !x.IsDeleted).ToListAsync(cancellationToken);
    }
}