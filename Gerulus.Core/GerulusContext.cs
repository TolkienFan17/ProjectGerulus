using Microsoft.EntityFrameworkCore;

namespace Gerulus.Core;

public class GerulusContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Message> Messages => Set<Message>();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=Gerulus.db");
    }
}
