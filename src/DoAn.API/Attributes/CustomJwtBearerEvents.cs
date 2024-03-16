using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace DoAn.API.Attributes;

public class CustomJwtBearerEvents : JwtBearerEvents
{
    // private readonly ICacheService _cacheService;
    //
    // public CustomJwtBearerEvents(ICacheService cacheService)
    // {
    //     _cacheService = cacheService;
    // }

    public override async Task TokenValidated(TokenValidatedContext context)
    {
        var requestToken = await context.HttpContext.GetTokenAsync("access_token");
        var a = 1;
        //var emailKey = accessToken.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Email)?.Value;
        // var authenticated = await _cacheService.GetAsync<Response.Authenticated>(emailKey);
        //
        // if (authenticated is null || authenticated.AccessToken != requestToken)
        // {
        //     context.Response.Headers.Add("IS-TOKEN-REVOKED", "true");
        //     context.Fail("Authentication fail. Token has been revoked!");
        // }
    }
}