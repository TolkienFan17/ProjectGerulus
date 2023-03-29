namespace Gerulus.Core;

public readonly record struct UserId(Guid Value) : IEquatable<UserId>
{
    public UserId() : this(Guid.NewGuid())
    {
    }
}
