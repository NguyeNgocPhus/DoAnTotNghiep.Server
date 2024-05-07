using DoAn.Application.Abstractions.Repositories;
using DoAn.Application.DTOs.ActionLog;
using DoAn.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DoAn.Persistence.Repositories;

public class ActionLogRepository : RepositoryBase<ActionLogs,Guid> , IActionLogRepository
{

    public ActionLogRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        
    }

    public async Task<List<ActionLogDto>> GetActionLog(Guid contextId, CancellationToken cancellationToken = default)
    {
        var query = from a in _dbContext.ActionLogs
            join u in _dbContext.AppUses on a.CreatedBy equals u.Id 
            where a.ContextId == contextId
            select new ActionLogDto()
            {
                ActivityId = a.ActivityId,
                ActivityName =  a.ActivityName,
                CreatedBy = a.CreatedBy,
                CreatedTime = a.CreatedTime,
                ContextId = a.Id,
                WorkflowInstanceId = a.WorkflowInstanceId,
                WorkflowDefinitionId = a.WorkflowDefinitionId,
                ActionReason = a.ActionReason,
                CreatedByName = u != null ?  u.UserName : "NONE"
                
            };
        return await query.ToListAsync(cancellationToken);

    }
}