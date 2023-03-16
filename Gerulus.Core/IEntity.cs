namespace Gerulus.Core;

public interface IEntity : IEntity<EntityId>
{
}

public interface IEntity<TId> where TId : IEquatable<TId>
{
    TId Id { get; }
}
