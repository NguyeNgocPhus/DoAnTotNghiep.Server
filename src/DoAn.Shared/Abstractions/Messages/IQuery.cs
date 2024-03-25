using DoAn.Shared.Abstractions.Shared;
using MediatR;

namespace DoAn.Shared.Abstractions.Messages;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
    
}