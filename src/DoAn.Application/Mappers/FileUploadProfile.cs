using AutoMapper;
using DoAn.Domain.Entities;
using DoAn.Shared.Services.V1.FileStorage.Responses;
using DoAn.Shared.Services.V1.ImportTemplate.Responses;

namespace DoAn.Application.Mappers;

public class FileUploadProfile : Profile
{
    public FileUploadProfile()
    {
        CreateMap<FileStorage, FileStorageResponse>();
    }
}