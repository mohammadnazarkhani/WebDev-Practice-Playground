using EFCoreShowcase.Entities.Base;

namespace EFCoreShowcase.Common.Events;

public interface IDomainEventHandler<in TEvent> where TEvent : DomainEvent
{
    Task HandleAsync(TEvent domainEvent, CancellationToken cancellationToken = default);
}
