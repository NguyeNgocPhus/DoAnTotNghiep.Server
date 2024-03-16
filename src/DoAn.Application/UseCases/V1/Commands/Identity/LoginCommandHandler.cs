using DoAn.Domain.Entities.Identity;
using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Abstractions.Shared;
using DoAn.Shared.Services.V1.Identity.Commands;
using DoAn.Shared.Services.V1.Identity.Responses;

namespace DoAn.Application.UseCases.V1.Commands.Identity;

public class LoginCommandHandler : ICommandHandler<LoginCommand, LoginResponse>
{
    public Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        
        throw new NotImplementedException();
    }
}