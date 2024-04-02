using AutoMapper;
using DoAn.Domain.Entities;
using DoAn.Shared.Services.V1.ImportTemplate.Responses;

namespace DoAn.Application.Mappers;

public class ImportTemplateProfile: Profile
{
    public ImportTemplateProfile()
    {
        CreateMap<ImportTemplate, ImportTemplateResponse>();
       
    }
}