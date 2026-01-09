using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NinaApp.Core.Mappers;
using NinaApp.Core.ServiceContracts;
using NinaApp.Core.Services;
using NinaApp.Core.Validators;

namespace NinaApp.Core
{
  public static class DependencyInjection
  {
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
      services.AddAutoMapper(typeof(UserToUserResponseMappingProfile).Assembly);

      services.AddScoped<IUsersService, UsersService>();

      services.AddValidatorsFromAssembly(typeof(UserCreationValidator).Assembly);
      return services;
    }
  }
}
