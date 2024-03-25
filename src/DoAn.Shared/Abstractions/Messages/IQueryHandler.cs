using DoAn.Shared.Abstractions.Shared;
using MediatR;

namespace DoAn.Shared.Abstractions.Messages;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery,Result<TResponse>> where TQuery : IQuery<TResponse>
{
    
}