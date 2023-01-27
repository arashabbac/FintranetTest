using System.Collections.Generic;

namespace Framework.Domain
{
    public interface IAggregateRoot : IEntity
    {
        void ClearDomainEvents();

        void ClearIntegrationEvents();

        IReadOnlyList<IDomainEvent> DomainEvents { get; }

        IReadOnlyList<IIntegrationEvent> IntegrationEvents { get; }
    }
}
