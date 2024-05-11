using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DoAn.Application.Abstractions.Repositories;
using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Abstractions.Shared;
using DoAn.Shared.Services.V1.ImportTemplate.Queries;
using DoAn.Shared.Services.V1.ImportTemplate.Responses;
using Microsoft.EntityFrameworkCore;

namespace DoAn.Application.UseCases.V1.Queries.ImportTemplate;

public class
    GetListImportTemplateQueryHandler : IQueryHandler<GetListImportTemplateQuery, PagedResult<ImportTemplateResponse>>
{
    private readonly IImportTemplateRepository _repository;
    private readonly IMapper _mapper;

    public GetListImportTemplateQueryHandler(IImportTemplateRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<PagedResult<ImportTemplateResponse>>> Handle(GetListImportTemplateQuery request,
        CancellationToken cancellationToken)
    {
        var query = _repository.GetImportTemplates(cancellationToken)
            .Where(x => !x.IsDeleted);

        if (!string.IsNullOrEmpty(request.Name))
        {
            query = query.Where(x => x.Name.Contains(request.Name));
        }
        if (!string.IsNullOrEmpty(request.Tag))
        {
            query = query.Where(x => x.Tag.Contains(request.Tag));
        }

        query = request.OrderByDesc
            ? query.OrderByDescending(GetSortProperty(request))
            : query.OrderBy(GetSortProperty(request));
        var importTemplates = await PagedResult<ImportTemplateResponse>.CreateAsync(query, request.Page, request.Size);


        return Result.Success(importTemplates);
    }

    private static Expression<Func<ImportTemplateResponse, object>> GetSortProperty(GetListImportTemplateQuery request)
        => request.OrderBy?.ToLower() switch
        {
            _ => ip => ip.CreatedTime
            //_ => product => product.CreatedDate // Default Sort Descending on CreatedDate column
        };
}