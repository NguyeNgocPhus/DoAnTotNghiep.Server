using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Azure;
using DoAn.API.Attributes;
using DoAn.Application.Abstractions;
using DoAn.Infrastructure.Authorization;
using DoAn.Infrastructure.DependencyInjection.Options;
using DoAn.Shared.Services.V1.Identity.Responses;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace DoAn.API.DependencyInjection.Extensions;

public static class JwtExtension
{
    public static void AddJwtAuthenticationAPI(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            JwtOption jwtOption = new JwtOption();
            configuration.GetSection(nameof(JwtOption)).Bind(jwtOption);

            /**
             * Storing the JWT in the AuthenticationProperties allows you to retrieve it from elsewhere within your application.
             * public async Task<IActionResult> SomeAction()
                {
                    // using Microsoft.AspNetCore.Authentication;
                    var accessToken = await HttpContext.GetTokenAsync("access_token");
                    // ...
                }
             */
            o.SaveToken = true; // Save token into AuthenticationProperties

            var Key = Encoding.UTF8.GetBytes(jwtOption.SecretKey);
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false, // on production make it true
                ValidateAudience = false, // on production make it true
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtOption.Issuer,
                ValidAudience = jwtOption.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Key),
                ClockSkew = TimeSpan.Zero
            };

            o.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    {
                        context.Response.Headers.Add("IS-TOKEN-EXPIRED", "true");
                    }

                    return Task.CompletedTask;
                },
                OnChallenge = context =>
                {
                    return Task.CompletedTask;
                },
                OnForbidden = (context) =>
                {
                    return Task.CompletedTask;
                },
                OnTokenValidated = async context =>
                {
                   
                    if (context.HttpContext.Request.Headers.TryGetValue("Authorization", out var accessToken))
                    {
                        var jwt = accessToken.ToString().Split(" ")[1];
                        
                        var cacheService =
                            (ICacheService)services.BuildServiceProvider().GetService(typeof(ICacheService))!;
                        var emailKey = context.Principal!.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Email)?.Value;
                        var authenticated = await cacheService.GetAsync<LoginResponse>(emailKey);
                        
                        if (authenticated is null || authenticated.AccessToken != jwt)
                        {
                            context.Response.Headers.Add("IS-TOKEN-REVOKED", "true");
                            context.Fail("Authentication fail. Token has been revoked!");
                        }
                        
                    }
                    else
                    {
                        context.Fail("Authentication fail.");
                    }

                    await Task.CompletedTask;
                }
            };

            //o.EventsType = typeof(CustomJwtBearerEvents);
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("AtLeast21", policy =>
                policy.Requirements.Add(new MinimumAgeRequirement(21)));
        });
        // services.AddScoped<CustomJwtBearerEvents>();
    }
}