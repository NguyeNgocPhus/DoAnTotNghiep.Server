using AutoMapper;
using DoAn.Application.Abstractions.Repositories;
using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Abstractions.Shared;
using DoAn.Shared.Services.V1.ImportTemplate.Queries;
using DoAn.Shared.Services.V1.ImportTemplate.Responses;

namespace DoAn.Application.UseCases.V1.Queries.ImportTemplate;

public class GetImportTemplateQueryHandler : IQueryHandler<GetImportTemplateQuery, ImportTemplateResponse>
{

    private readonly IImportTemplateRepository _repository;
    private readonly IMapper _mapper;

    public GetImportTemplateQueryHandler(IImportTemplateRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<ImportTemplateResponse>> Handle(GetImportTemplateQuery request, CancellationToken cancellationToken)
    {
        var importTemplate =await _repository.FindByIdAsync(request.Id, cancellationToken);
        var result = _mapper.Map<ImportTemplateResponse>(importTemplate);
        return Result.Success(result);
    }
}