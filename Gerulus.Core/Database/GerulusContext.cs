using System.ComponentModel.DataAnnotations;
using Gerulus.Core.Contacts;
using Gerulus.Core.Database;
using Microsoft.EntityFrameworkCore;

namespace Gerulus.Core.Database;

public class GerulusContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<Contact> Contacts => Set<Contact>();

    protected IDatabaseOptionsProvider OptionsProvider { get; private init; }

    public GerulusContext(IDatabaseOptionsProvider optionsProvider)
    {
        OptionsProvider = optionsProvider;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // TODO Find a way to refactor this to use async operations directly.
        var dbOptions = OptionsProvider.ReadAsync().ConfigureAwait(false).GetAwaiter().GetResult();

        if (dbOptions.IsInMemory)
            options.UseInMemoryDatabase("Gerulus");
        else
            options.UseSqlite(dbOptions.ConnectionString);
    }
}
