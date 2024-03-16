using DoAn.Shared.Abstractions.Shared;
using MediatR;

namespace DoAn.Shared.Abstractions.Messages;

public interface ICommand : IRequest<Result>
{
    
}
public interface ICommand<T> : IRequest<Result<T>>
{
    
}