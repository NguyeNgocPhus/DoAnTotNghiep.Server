using AutoMapper;
using DoAn.Application.Abstractions;
using DoAn.Application.Abstractions.Repositories;
using DoAn.Domain.Entities;
using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Abstractions.Shared;
using DoAn.Shared.Services.V1.FileStorage.Queries;
using DoAn.Shared.Services.V1.FileStorage.Responses;
using FileNotFoundException = DoAn.Application.Exceptions.FileNotFoundException;

namespace DoAn.Application.UseCases.V1.Queries.FileUpload;

public class GetFileStorageQueryHandler : IQueryHandler<GetFileStorageQuery, FileStorageResponse>
{
    private readonly IRepositoryBase<FileStorage, Guid> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetFileStorageQueryHandler(IRepositoryBase<FileStorage, Guid> repository, IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<FileStorageResponse>> Handle(GetFileStorageQuery request,
        CancellationToken cancellationToken)
    {
        var fileStorage = await _repository.FindByIdAsync(request.Id, cancellationToken);
        if (fileStorage == null)
        {
            throw new FileNotFoundException("File storage not found");
        }
        return _mapper.Map<FileStorageResponse>(fileStorage);
    }
}