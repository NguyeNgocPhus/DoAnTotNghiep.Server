using DoAn.Application.Abstractions.Repositories;
using DoAn.Shared.Abstractions.Messages;
using Newtonsoft.Json;
using Quartz;

namespace DoAn.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public class NotificationBackgroundJobs : IJob
{
    private readonly IRepositoryBase<Domain.Entities.Notification, int> _repository;

    public NotificationBackgroundJobs(IRepositoryBase<Domain.Entities.Notification, int> repository)
    {
        _repository = repository;
    }


    public async Task Execute(IJobExecutionContext context)
    {
    }
}