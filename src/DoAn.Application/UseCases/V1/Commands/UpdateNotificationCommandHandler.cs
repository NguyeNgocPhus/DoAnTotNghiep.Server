using DoAn.Application.Abstractions;
using DoAn.Application.Abstractions.Repositories;
using DoAn.Domain.Entities;
using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Abstractions.Shared;
using DoAn.Shared.Services.V1.Notification;

namespace DoAn.Application.UseCases.V1.Commands;

public class UpdateNotificationCommandHandler : ICommandHandler<UpdateNotificationCommand>
{
    private readonly IRepositoryBase<Notification, int> _repositoryBase;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateNotificationCommandHandler(IRepositoryBase<Notification, int> repositoryBase, IUnitOfWork unitOfWork)
    {
        _repositoryBase = repositoryBase;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateNotificationCommand request, CancellationToken cancellationToken)
    {
       var notification = await _repositoryBase.FindByIdAsync(request.Id, cancellationToken);
       
       notification.Read = true;
       await _unitOfWork.SaveChangesAsync(cancellationToken);
       
       return Result.Success();
    }
}