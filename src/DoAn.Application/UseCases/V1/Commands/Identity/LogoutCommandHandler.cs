using DoAn.Application.Abstractions;
using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Abstractions.Shared;
using DoAn.Shared.Services.V1.Identity.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn.Application.UseCases.V1.Commands.Identity
{
    public class LogoutCommandHandler : ICommandHandler<LogoutCommand>
    {
        private readonly ICacheService _cacheService;
        public LogoutCommandHandler(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            await _cacheService.RemoveAsync(request.Email, cancellationToken);
            return Result.Success();
        }
    }
}
