using Microsoft.EntityFrameworkCore;

namespace Gerulus.Core;

public class GerulusContext : DbContext
{
    public DbSet<User> Users { get; }
}
