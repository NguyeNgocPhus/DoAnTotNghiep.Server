using DoAn.Domain.Entities;
using DoAn.Shared.Services.V1.ImportTemplate.Responses;

namespace DoAn.Application.Abstractions.Repositories;

public interface IImportTemplateRepository : IRepositoryBase<ImportTemplate,Guid>
{
    Task<ImportTemplate?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    IQueryable<ImportTemplateResponse> GetImportTemplates(CancellationToken cancellationToken = default);
    Task<List<ImportTemplate>> FindIgnoreDelete(CancellationToken cancellationToken = default);
}