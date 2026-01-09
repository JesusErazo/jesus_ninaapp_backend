using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NinaApp.Core.RepositoryContracts;
using NinaApp.Infrastructure.Database;
using NinaApp.Infrastructure.Repositories;

namespace NinaApp.Infrastructure
{
  public static class DependencyInjection
  {
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
      string connectionString = configuration.GetConnectionString("SqlServer") ??
        throw new ArgumentNullException("SqlServer connection string is missing.");

      services.AddDbContext<ApplicationDbContext>(options =>
      {
        options.UseSqlServer(connectionString);
      });

      services.AddScoped<IUsersRepository, UsersRepository>();

      return services;
    }
  }
}
