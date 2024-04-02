using AutoMapper;
using DoAn.Application.Abstractions.Repositories;
using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Abstractions.Shared;
using DoAn.Shared.Services.V1.ImportTemplate.Queries;
using DoAn.Shared.Services.V1.ImportTemplate.Responses;
using Microsoft.EntityFrameworkCore;

namespace DoAn.Application.UseCases.V1.Queries.ImportTemplate;

public class GetListImportTemplateQueryHandler : IQueryHandler<GetListImportTemplateQuery, List<ImportTemplateResponse>>
{
    private readonly IImportTemplateRepository _repository;
    private readonly IMapper _mapper;

    public GetListImportTemplateQueryHandler(IImportTemplateRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<List<ImportTemplateResponse>>> Handle(GetListImportTemplateQuery request,
        CancellationToken cancellationToken)
    {
        var importTemplate = await _repository.FindAll().ToListAsync(cancellationToken);
        var result = _mapper.Map<List<ImportTemplateResponse>>(importTemplate);
        return Result.Success(result);
    }
}