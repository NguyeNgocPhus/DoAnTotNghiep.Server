using MediatR;

namespace DoAn.Shared.Abstractions.Messages;

public interface IQuery<TResponse> : IRequest<TResponse>
{
    
}