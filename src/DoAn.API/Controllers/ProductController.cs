using DoAn.Application.Abstractions;
using DoAn.Shared.Services.V1.Identity.Commands;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoAn.API.Controllers;

public class ProductController : ApiControllerBase
{
    private readonly IMediator _mediator;
    private readonly IJwtTokenService _jwtTokenService;

    public ProductController(IMediator mediator, IJwtTokenService jwtTokenService)
    {
        _mediator = mediator;
        _jwtTokenService = jwtTokenService;
    }

    [HttpGet]
    [Route("")]
    [Authorize]
    public async Task<ActionResult> Get(CancellationToken cancellationToken)
    {
        var token =await HttpContext.GetTokenAsync("access_token");
        var claims = _jwtTokenService.GetPrincipalFromExpiredToken(token);
        return Ok("result.Value");
    }
   
}