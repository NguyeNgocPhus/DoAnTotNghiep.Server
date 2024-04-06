using AutoMapper;
using DoAn.Application.Abstractions;
using DoAn.Application.Abstractions.Repositories;
using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Abstractions.Shared;
using DoAn.Shared.Services.V1.ImportTemplate.Commands;
using DoAn.Shared.Services.V1.ImportTemplate.Responses;

namespace DoAn.Application.UseCases.V1.Commands.ImportTemplate;

public class CreateImportTemplateCommandHandler : ICommandHandler<CreateImportTemplateCommand, CreateImportTemplateResponse>
{

    private readonly IImportTemplateRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateImportTemplateCommandHandler(IImportTemplateRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }


    public async Task<Result<CreateImportTemplateResponse>> Handle(CreateImportTemplateCommand request, CancellationToken cancellationToken)
    {
        var duplicateImportTemplate = await _repository.GetByNameAsync(request.Name, cancellationToken);
        if (duplicateImportTemplate != null)
        {
            
        }
        var importTemplate = new Domain.Entities.ImportTemplate()
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Active = true,
            Description = request.Description,
            CreatedBy = Guid.NewGuid(),
            Tag = request.Tag,
            CreatedTime = DateTime.Now,
            DisplayOrder = 1,
            HasWorkflow = false,

        };
        
        _repository.Add(importTemplate);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var result = _mapper.Map<CreateImportTemplateResponse>(importTemplate);
        return Result.Success(result);
    }
}