namespace Gerulus.Core.Database;

public record DatabaseOptions
{
    public required string ConnectionString { get; init; }
    public bool IsInMemory { get; init; } = false;
}
