using System.Xml.Linq;
using AutoMapper;
using DoAn.Application.Abstractions.Repositories;
using DoAn.Domain.Entities;
using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Abstractions.Shared;
using DoAn.Shared.Services.V1.ImportHistory.Queries;
using DoAn.Shared.Services.V1.ImportHistory.Responses;
using Microsoft.EntityFrameworkCore;

namespace DoAn.Application.UseCases.V1.Queries.ImportHistories;

public class GetListImportHistoryQueryHandler : IQueryHandler<GetListImportHistoryQuery, List<ImportHistoryResponse>>
{
    private readonly IRepositoryBase<ImportHistory, Guid> _repository;

    private readonly IMapper _mapper;

    public GetListImportHistoryQueryHandler(IRepositoryBase<ImportHistory, Guid> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<List<ImportHistoryResponse>>> Handle(GetListImportHistoryQuery request,
        CancellationToken cancellationToken)
    {
        var importTemplate =  _repository
            .FindAll()
            .Where(x => !x.IsDeleted)
            .Include(x => x.User)
            .Include(x => x.ImportTemplate)
            .Select(x => new ImportHistoryResponse()
            {
                Id = x.Id,
                Status = x.Status,
                CreatedTime = x.CreatedTime,
                ImportTemplateId = x.ImportTemplateId,
                ImportTemplateName = x.ImportTemplate.Name,
                FileId = x.FileId,
                CreatedBy = x.CreatedBy,
                CreatedByName = x.User.UserName
            });
            


        var result = await importTemplate.ToListAsync(cancellationToken);
        return Result.Success(result);
    }
}