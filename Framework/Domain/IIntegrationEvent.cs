using MediatR;
using System;

namespace Framework.Domain;

public interface IIntegrationEvent : INotification
{
    public Guid EventId { get; }
}