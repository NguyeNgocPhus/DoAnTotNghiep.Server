using DoAn.API.DependencyInjection.Options;
using DoAn.Application.Exceptions;
using DoAn.Shared.Services.V1.Dashboard;
using DoAn.Shared.Services.V1.FileStorage.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetBox.Extensions;

namespace DoAn.API.Controllers;

public class DashboardController: ApiControllerBase
{
    private readonly IMediator _mediator;
    private readonly FileConfiguration _fileConfiguration = new FileConfiguration();
  
    public DashboardController(IConfiguration configuration, IMediator mediator)
    {
        _mediator = mediator;
        configuration.GetSection(nameof(FileConfiguration)).Bind(_fileConfiguration);
    }
    [HttpGet]
    [Route(Common.Url.ADMIN.Dashboard.View)]
    // [Permissions]
    public async Task<IActionResult> ViewDashboard([FromQuery] long date, CancellationToken cancellationToken = default)
    {
        
        var result = await _mediator.Send(new ViewDashboardQuery()
        {
            Date = DateTime.UnixEpoch.AddMilliseconds(date)
        }, cancellationToken);
        if (!result.IsSuccess)
        {
            
        }
        return Ok(result);
    }
}