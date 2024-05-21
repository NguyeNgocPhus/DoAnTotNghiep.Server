using DoAn.Application.Abstractions.Repositories;
using DoAn.Domain.Entities;
using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Abstractions.Shared;
using DoAn.Shared.Services.V1.Dashboard;
using Microsoft.EntityFrameworkCore;

namespace DoAn.Application.UseCases.V1.Queries;

public class ViewDashboardQueryHandler : IQueryHandler<ViewDashboardQuery, ViewDashboardResponse>
{
    private readonly IRepositoryBase<ActionLogs, Guid> _repository;

    public ViewDashboardQueryHandler(IRepositoryBase<ActionLogs, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<Result<ViewDashboardResponse>> Handle(ViewDashboardQuery request, CancellationToken cancellationToken)
    {
        var countUploadResponse = DashboardDto.initCoutUpload();
        var countApproveResponse = DashboardDto.initCoutApprove();
        var countRejectResponse = DashboardDto.initCoutReject();
        
        
        var uploads = await _repository.AsQueryable()
            .Where(x => x.ActivityName == "FileUpload"
                        && x.CreatedTime.Day == request.Date.Day && x.CreatedTime.Month == request.Date.Month &&
                        x.CreatedTime.Year == request.Date.Year)
            .ToListAsync(cancellationToken);
        var approves = await _repository.AsQueryable()
            .Where(x => x.ActivityName == "Approve"
                        && x.CreatedTime.Day == request.Date.Day && x.CreatedTime.Month == request.Date.Month &&
                        x.CreatedTime.Year == request.Date.Year)
            .ToListAsync(cancellationToken);
        var rejects = await _repository.AsQueryable()
            .Where(x => x.ActivityName == "Reject"
                        && x.CreatedTime.Day == request.Date.Day && x.CreatedTime.Month == request.Date.Month &&
                        x.CreatedTime.Year == request.Date.Year)
            .ToListAsync(cancellationToken);

        countUploadResponse = countUploadResponse.Select(x =>
        {
            var e = uploads.Where(u => u.CreatedTime.Hour.ToString() == x.Hour).ToList();
            x.Value = e.Count();
            x.Hour = $"{x.Hour}h";
            return x;
        }).ToList();
        countApproveResponse = countApproveResponse.Select(x =>
        {
            var e = approves.Where(u => u.CreatedTime.Hour.ToString() == x.Hour).ToList();
            x.Value = e.Count();
            x.Hour = $"{x.Hour}h";
            return x;
        }).ToList();
        countRejectResponse = countRejectResponse.Select(x =>
        {
            var e = rejects.Where(u => u.CreatedTime.Hour.ToString() == x.Hour).ToList();
            x.Value = e.Count();
            x.Hour = $"{x.Hour}h";
            return x;
        }).ToList();
        var result = new ViewDashboardResponse()
        {
            Approves = countApproveResponse,
            Rejects = countRejectResponse,
            Uploads = countUploadResponse,
            CountUpload = uploads.Count,
            CountApprove = approves.Count,
            CountReject = rejects.Count
        };
        return Result.Create(result);
    }
}