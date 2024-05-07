using AutoMapper;
using DoAn.Application.Abstractions;
using DoAn.Application.Abstractions.Repositories;
using DoAn.Domain.Entities;
using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Abstractions.Shared;
using DoAn.Shared.Extensions;
using DoAn.Shared.Services.V1.FileStorage.Commands;
using DoAn.Shared.Services.V1.FileStorage.Responses;

namespace DoAn.Application.UseCases.V1.Commands.FileUpload;

public class UploadFileCommandHandler : ICommandHandler<UploadFileCommand, FileStorageResponse>
{
    private readonly IRepositoryBase<FileStorage, Guid> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public UploadFileCommandHandler(IRepositoryBase<FileStorage, Guid> repository, IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<Result<FileStorageResponse>> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        var entry = new FileStorage()
        {
            Id = Guid.NewGuid(),
            Data = request.Data,
            Name = $"{Guid.NewGuid()}_{request.Name.GetFileExtension()}",
            OriginalName = request.Name,
            Size = (long) request.Length,
            Status = "",
            MimeType = request.MimeType,
            CreatedBy = Guid.Parse(_currentUserService.UserId),
            CreatedTime = DateTime.Now,
        };
        _repository.Add(entry);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var result = _mapper.Map<FileStorageResponse>(entry);
        result.Data = null; // ignore data file xlsx
        return Result.Success(result);
    }
}