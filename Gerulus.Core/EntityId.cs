namespace Gerulus.Core;

public readonly record struct EntityId(Guid Value) : IEquatable<EntityId>
{
    public EntityId() : this(Guid.NewGuid())
    {
    }
}