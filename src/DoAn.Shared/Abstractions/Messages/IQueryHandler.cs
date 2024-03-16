using MediatR;

namespace DoAn.Shared.Abstractions.Messages;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery,TResponse> where TQuery : IQuery<TResponse>
{
    
}