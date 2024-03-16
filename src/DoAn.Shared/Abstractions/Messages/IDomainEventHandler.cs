using MediatR;

namespace DoAn.Shared.Abstractions.Messages;

public interface IDomainEventHandler<TDomainEvent> : INotificationHandler<TDomainEvent> where TDomainEvent : IDomainEvent
{
    
}