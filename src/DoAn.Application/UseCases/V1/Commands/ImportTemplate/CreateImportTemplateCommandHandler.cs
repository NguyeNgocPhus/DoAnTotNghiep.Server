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

    public CreateImportTemplateCommandHandler(IImportTemplateRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
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
            Tag = "TCKT",
            CreatedTime = DateTime.Now,
            DisplayOrder = 1,
            HasWorkflow = false,
            
        }
            
        ;
        _repository.Add(importTemplate);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(new CreateImportTemplateResponse()
        {
            Id = importTemplate.Id
        });
    }
}