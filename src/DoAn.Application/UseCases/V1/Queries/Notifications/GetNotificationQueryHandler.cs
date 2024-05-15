using AutoMapper;
using AutoMapper.QueryableExtensions;
using DoAn.Application.Abstractions;
using DoAn.Application.Abstractions.Repositories;
using DoAn.Domain.Entities;
using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Abstractions.Shared;
using DoAn.Shared.Services.V1.Notification;

namespace DoAn.Application.UseCases.V1.Queries.Notifications;

public class GetNotificationQueryHandler: IQueryHandler<GetNotificationQuery, PagedResult<NotificationResponse>>
{
    private readonly IRepositoryBase<Notification, int> _repository;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetNotificationQueryHandler(IRepositoryBase<Notification, int> repository, IMapper mapper, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<Result<PagedResult<NotificationResponse>>> Handle(GetNotificationQuery request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;
        var query = _repository.AsQueryable()
            .Where(x => x.UserId == Guid.Parse(currentUserId))
            .ProjectTo<NotificationResponse>(_mapper.ConfigurationProvider)
            .OrderByDescending(x => x.CreatedTime);
        var notifications = await PagedResult<NotificationResponse>.CreateAsync(query, request.Page, request.Size);

        return notifications;
    }
}