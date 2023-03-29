namespace Gerulus.Core;

public abstract class AggregateRoot : AggregateRoot<EntityId>
{
    protected AggregateRoot(EntityId? id = null) : base(id ?? new EntityId())
    {
    }
}

public abstract class AggregateRoot<TId> : IEntity<TId> where TId : IEquatable<TId>
{
    public TId Id { get; }

    protected AggregateRoot(TId id)
    {
        Id = id;
    }

    protected List<IDomainEvent> Events { get; } = new();

    public IDomainEvent[] RetrieveEvents()
    {
        var events = Events.ToArray();
        Events.Clear();
        return events;
    }

    public override bool Equals(object? other)
    {
        if (other is AggregateRoot root)
        {
            return root.Id.Equals(Id);
        }

        return false;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
