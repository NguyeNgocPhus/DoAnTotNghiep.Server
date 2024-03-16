using DoAn.Shared.Abstractions.Shared;
using DoAn.Shared.Services.V1.Identity.Commands;
using MediatR;

namespace DoAn.Shared.Abstractions.Messages;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result> where TCommand : ICommand
{
}

public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>
{
}