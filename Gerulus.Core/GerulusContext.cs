using Gerulus.Core.Contacts;
using Microsoft.EntityFrameworkCore;

namespace Gerulus.Core;

public class GerulusContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<Contact> Contacts => Set<Contact>();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=Gerulus.db");
    }
}
