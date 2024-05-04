using DoAn.Application.Abstractions;
using DoAn.Application.Abstractions.Repositories;
using DoAn.Application.DTOs.Workflow;
using DoAn.Application.Exceptions;
using DoAn.Domain.Entities;
using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Abstractions.Shared;
using DoAn.Shared.Services.V1.ImportTemplate.Commands;
using FileNotFoundException = DoAn.Application.Exceptions.FileNotFoundException;

namespace DoAn.Application.UseCases.V1.Commands.ImportTemplate;

public class ImportDataCommandHandler : ICommandHandler<ImportDataCommand>
{
    private readonly IRepositoryBase<FileStorage, Guid> _fileStorageRepository;
    private readonly IRepositoryBase<ImportHistory, Guid> _importHistoryRepository;
    private readonly IRepositoryBase<Domain.Entities.ImportTemplate, Guid> _importTemplateRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IWorkflowLaunchpadService _workflowLaunchpadService;
    

    public ImportDataCommandHandler(IRepositoryBase<FileStorage, Guid> fileStorageRepository,
        IRepositoryBase<ImportHistory, Guid> importHistoryRepository,
        IRepositoryBase<Domain.Entities.ImportTemplate, Guid> importTemplateRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IWorkflowLaunchpadService workflowLaunchpadService)
    {
        _fileStorageRepository = fileStorageRepository;
        _importHistoryRepository = importHistoryRepository;
        _importTemplateRepository = importTemplateRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _workflowLaunchpadService = workflowLaunchpadService;
    }

    public async Task<Result> Handle(ImportDataCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        var importTemplate = await _importTemplateRepository.FindByIdAsync(request.ImportTemplateId, cancellationToken);
        if (importTemplate is null)
        {
            throw new ImportTemplateException("import template is not found");
        }

        var file = await _fileStorageRepository.FindByIdAsync(request.FileUploadId, cancellationToken);
        if (file is null)
        {
            throw new FileNotFoundException("file is not found");
        }
        
        
        var importHistory = new ImportHistory()
        {
            Id = Guid.NewGuid(),
            ImportTemplateId = importTemplate.Id,
            UserId = Guid.Parse(userId),
            CreatedBy = Guid.Parse(userId),
            CreatedTime = DateTime.Now,
            Status = "PENDING",
            Version = 1,
            FileId = file.Id

        };
        _importHistoryRepository.Add(importHistory);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        // start workflow
        await _workflowLaunchpadService.StartWorkflowsAsync(new ExecuteFileUpdateDto()
        {
            FileId = file.Id,
            ImportTemplateId = importTemplate.Id.ToString()
        },cancellationToken);
        
        return Result.Success();

    }
}