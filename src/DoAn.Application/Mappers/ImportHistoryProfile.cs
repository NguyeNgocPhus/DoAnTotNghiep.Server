using AutoMapper;
using DoAn.Domain.Entities;
using DoAn.Shared.Services.V1.ImportHistory.Responses;
using DoAn.Shared.Services.V1.ImportTemplate.Responses;

namespace DoAn.Application.Mappers;

public class ImportHistoryProfile : Profile
{
    public ImportHistoryProfile()
    {
        CreateMap<ImportHistory, ImportHistoryResponse>();
    }
}