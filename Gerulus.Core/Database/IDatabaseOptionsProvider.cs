namespace Gerulus.Core.Database;

public interface IDatabaseOptionsProvider
{
    Task<DatabaseOptions> ReadAsync();
}
