using DoAn.Application.Abstractions;
using DoAn.Application.Abstractions.Repositories;
using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Abstractions.Shared;
using DoAn.Shared.Services.V1.ImportTemplate.Commands;
using DoAn.Shared.Services.V1.ImportTemplate.Responses;

namespace DoAn.Application.UseCases.V1.Commands.ImportTemplate;

public class DeleteImportTemplateCommandHandler : ICommandHandler<DeleteImportTemplateCommand, DeleteImportTemplateResponse>
{
    private readonly IImportTemplateRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteImportTemplateCommandHandler(IImportTemplateRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<DeleteImportTemplateResponse>> Handle(DeleteImportTemplateCommand request, CancellationToken cancellationToken)
    {
        var importTemplate = await _repository.FindByIdAsync(request.Id, cancellationToken);
        if (importTemplate == null)
        {
            
        }

        importTemplate!.IsDeleted = true;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(new DeleteImportTemplateResponse()
        {
            Id = importTemplate.Id
        });
    }
}