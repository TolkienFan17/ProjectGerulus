namespace Gerulus.Core.Database;

public class DefaultDatabaseOptionsProvider : IDatabaseOptionsProvider
{
    public Task<DatabaseOptions> ReadAsync() => Task.FromResult(new DatabaseOptions()
    {
        ConnectionString = "Data Source=Gerulus.db",
        IsInMemory = false
    });
}
