using AutoMapper;
using DoAn.Application.Abstractions;
using DoAn.Application.Abstractions.Repositories;
using DoAn.Domain.Entities;
using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Abstractions.Shared;
using DoAn.Shared.Services.V1.Notification;
using Microsoft.EntityFrameworkCore;

namespace DoAn.Application.UseCases.V1.Queries.Notifications;

public class GetCountUnreadNotificationQueryHandler: IQueryHandler<GetCountUnreadNotificationQuery, int>
{
    private readonly IRepositoryBase<Notification, int> _repository;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetCountUnreadNotificationQueryHandler(IRepositoryBase<Notification, int> repository, IMapper mapper, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<Result<int>> Handle(GetCountUnreadNotificationQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        var count= await _repository.AsQueryable().Where(x => x.UserId == Guid.Parse(userId) && !x.Read).CountAsync(cancellationToken);
        return Result.Success(count);
    }
}