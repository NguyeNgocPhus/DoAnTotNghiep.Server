using System.Linq.Expressions;
using System.Xml.Linq;
using AutoMapper;
using DoAn.Application.Abstractions.Repositories;
using DoAn.Domain.Entities;
using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Abstractions.Shared;
using DoAn.Shared.Services.V1.Identity.Queries;
using DoAn.Shared.Services.V1.Identity.Responses;
using DoAn.Shared.Services.V1.ImportHistory.Queries;
using DoAn.Shared.Services.V1.ImportHistory.Responses;
using Microsoft.EntityFrameworkCore;

namespace DoAn.Application.UseCases.V1.Queries.ImportHistories;

public class GetListImportHistoryQueryHandler : IQueryHandler<GetListImportHistoryQuery, PagedResult<ImportHistoryResponse>>
{
    private readonly IRepositoryBase<ImportHistory, Guid> _repository;

    private readonly IMapper _mapper;

    public GetListImportHistoryQueryHandler(IRepositoryBase<ImportHistory, Guid> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<PagedResult<ImportHistoryResponse>>> Handle(GetListImportHistoryQuery request,
        CancellationToken cancellationToken)
    {
        var query =  _repository
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
        if (request.ImportTemplateId != null)
        {
            query = query.Where(x => x.ImportTemplateId == request.ImportTemplateId);
        }

        if (request.Status != null)
        {
            query = query.Where(x => x.Status == request.Status);
        }
        
        
        
        query = request.OrderByDesc
            ? query.OrderByDescending(GetSortProperty(request))
            : query.OrderBy(GetSortProperty(request));   
        
        var result = await PagedResult<ImportHistoryResponse>.CreateAsync(query, request.Page, request.Size);

     
        return Result.Success(result);
    }
    private static Expression<Func<ImportHistoryResponse, object>> GetSortProperty(GetListImportHistoryQuery request)
        => request.OrderBy?.ToLower() switch
        {
           
            _ => product => product.CreatedTime
            //_ => product => product.CreatedDate // Default Sort Descending on CreatedDate column
        };
}