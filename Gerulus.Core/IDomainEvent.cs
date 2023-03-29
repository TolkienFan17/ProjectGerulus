using MediatR;

namespace Gerulus.Core;

public interface IDomainEvent : INotification
{
    DateTimeOffset Timestamp { get; }
}