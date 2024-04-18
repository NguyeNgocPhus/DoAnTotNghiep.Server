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
    private readonly IRepositoryBase<ImportHistory,Guid> _repository;
    
    private readonly IMapper _mapper;

    public GetListImportHistoryQueryHandler( IRepositoryBase<ImportHistory,Guid> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<List<ImportHistoryResponse>>> Handle(GetListImportHistoryQuery request,
        CancellationToken cancellationToken)
    {
        var importTemplate = await _repository.FindAll().Where(x => !x.IsDeleted).ToListAsync(cancellationToken);
        var result = _mapper.Map<List<ImportHistoryResponse>>(importTemplate);
        return Result.Success(result);
    }
}