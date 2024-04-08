using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace DoAn.Infrastructure.Authorization;

public class MinimumAgeHandler : AuthorizationHandler<MinimumAgeRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        MinimumAgeRequirement requirement)
    {
        var dateOfBirthClaim = context.User.FindFirst(
            c => c.Type == ClaimTypes.Email);

        if (dateOfBirthClaim is null)
        {
            return Task.CompletedTask;
        }

        // var dateOfBirth = Convert.ToDateTime(dateOfBirthClaim.Value);
        // int calculatedAge = DateTime.Today.Year - dateOfBirth.Year;
        // if (dateOfBirth > DateTime.Today.AddYears(-calculatedAge))
        // {
        //     calculatedAge--;
        // }

        if (dateOfBirthClaim.Value == requirement.MinimumAge.ToString())
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}