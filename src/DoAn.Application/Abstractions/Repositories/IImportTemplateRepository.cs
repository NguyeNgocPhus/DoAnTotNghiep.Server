using DoAn.Domain.Entities;

namespace DoAn.Application.Abstractions.Repositories;

public interface IImportTemplateRepository : IRepositoryBase<ImportTemplate,Guid>
{
    Task<ImportTemplate?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<List<ImportTemplate>> FindIgnoreDelete(CancellationToken cancellationToken = default);
}