using Gerulus.Core.Config;

namespace Gerulus.Core.Database;

public class DefaultDatabaseOptionsProvider : IDatabaseOptionsProvider
{
    public required IConfigProvider Config { get; init; }

    public Task<DatabaseOptions> ReadAsync() => Task.FromResult(new DatabaseOptions()
    {
        ConnectionString = $"Data Source={Config.Get().DatabaseFileLocation}",
        IsInMemory = false
    });
}
