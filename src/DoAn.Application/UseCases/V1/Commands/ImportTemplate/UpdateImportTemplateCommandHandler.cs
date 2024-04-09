using DoAn.Application.Abstractions;
using DoAn.Application.Abstractions.Repositories;
using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Abstractions.Shared;
using DoAn.Shared.Services.V1.ImportTemplate.Commands;
using DoAn.Shared.Services.V1.ImportTemplate.Responses;

namespace DoAn.Application.UseCases.V1.Commands.ImportTemplate;

public class UpdateImportTemplateCommandHandler : ICommandHandler<UpdateImportTemplateCommand,UpdateImportTemplateResponse>
{
    private readonly IImportTemplateRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    

    public UpdateImportTemplateCommandHandler(IImportTemplateRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<UpdateImportTemplateResponse>> Handle(UpdateImportTemplateCommand request, CancellationToken cancellationToken)
    {
        var importTemplate = await _repository.FindByIdAsync(request.Id, cancellationToken);
        if (importTemplate == null)
        {
            
        }

        importTemplate.Name = request.Name;
        importTemplate.Tag = request.Tag;
        importTemplate.Description = request.Description;
        importTemplate.DisplayOrder = request.DisplayOrder;
        importTemplate.UpdatedBy = Guid.NewGuid();
        importTemplate.UpdatedTime = DateTime.Now;
        importTemplate.FileTemplateId = request.FileTemplateId;
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        
        return Result.Success(new UpdateImportTemplateResponse()
        {
            Id = importTemplate.Id
        });

    }
}