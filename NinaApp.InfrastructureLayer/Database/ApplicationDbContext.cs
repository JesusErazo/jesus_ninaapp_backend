using Microsoft.EntityFrameworkCore;
using NinaApp.Core.Entities;

namespace NinaApp.Infrastructure.Database
{
  public class ApplicationDbContext : DbContext
  {
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

  }
}
