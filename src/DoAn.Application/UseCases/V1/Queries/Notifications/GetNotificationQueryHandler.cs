using AutoMapper;
using AutoMapper.QueryableExtensions;
using DoAn.Application.Abstractions;
using DoAn.Application.Abstractions.Repositories;
using DoAn.Domain.Entities;
using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Abstractions.Shared;
using DoAn.Shared.Services.V1.Notification;
using Microsoft.EntityFrameworkCore;

namespace DoAn.Application.UseCases.V1.Queries.Notifications;

public class GetNotificationQueryHandler: IQueryHandler<GetNotificationQuery, List<NotificationResponse>>
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

    public async Task<Result<List<NotificationResponse>>> Handle(GetNotificationQuery request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;
        var query = _repository.AsQueryable()
            .Where(x => x.UserId == Guid.Parse(currentUserId))
            .ProjectTo<NotificationResponse>(_mapper.ConfigurationProvider)
            .OrderByDescending(x => x.CreatedTime)
            .Take(request.Page * request.Size);
        var notifications = await query.ToListAsync(cancellationToken);
    
        return notifications;
    }
}