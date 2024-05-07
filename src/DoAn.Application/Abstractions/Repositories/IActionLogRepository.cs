using DoAn.Application.DTOs.ActionLog;
using DoAn.Domain.Entities;

namespace DoAn.Application.Abstractions.Repositories;

public interface IActionLogRepository
{
    Task<List<ActionLogDto>> GetActionLog(Guid contextId, CancellationToken cancellationToken = default);
    
}