using System.Collections.Generic;

namespace Framework.Domain
{
    public abstract class AggregateRoot<TKey> : 
        Entity<TKey>, IAggregateRoot
    {
        protected AggregateRoot()
        {
            _domainEvents = new List<IDomainEvent>();
            _integrationEvents = new List<IIntegrationEvent>();
        }

        // **********
        [System.Text.Json.Serialization.JsonIgnore]
        private readonly List<IDomainEvent> _domainEvents;

        [System.Text.Json.Serialization.JsonIgnore]
        public IReadOnlyList<IDomainEvent> DomainEvents
        {
            get
            {
                return _domainEvents;
            }
        }
        // **********

        // **********
        [System.Text.Json.Serialization.JsonIgnore]
        private readonly List<IIntegrationEvent> _integrationEvents;

        [System.Text.Json.Serialization.JsonIgnore]
        public IReadOnlyList<IIntegrationEvent> IntegrationEvents
        {
            get
            {
                return _integrationEvents;
            }
        }
        // **********

        protected void RaiseDomainEvent(IDomainEvent domainEvent)
        {
            if (domainEvent is null)
            {
                return;
            }

            _domainEvents?.Add(domainEvent);
        }

        protected void RemoveDomainEvent(IDomainEvent domainEvent)
        {
            if (domainEvent is null)
            {
                return;
            }

            _domainEvents?.Remove(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        protected void RaiseIntegrationEvent(IIntegrationEvent integration)
        {
            if (integration is null)
            {
                return;
            }

            _integrationEvents?.Add(integration);
        }

        protected void RemoveIntegrationEvent(IIntegrationEvent integration)
        {
            if (integration is null)
            {
                return;
            }

            _integrationEvents?.Remove(integration);
        }

        public void ClearIntegrationEvents()
        {
            _integrationEvents?.Clear();
        }
    }
}
