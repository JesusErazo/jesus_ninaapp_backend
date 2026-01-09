using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace NinaApp.API.Extensions
{
  public static class LocalizationExtensions
  {
    public static IServiceCollection AddApiLocalization(this IServiceCollection services)
    {
      services.Configure<RequestLocalizationOptions>(options =>
      {
        var supportedCultures = new[]
        {
          new CultureInfo("en-US"),
          new CultureInfo("es-ES"),
          new CultureInfo("es")
        };

        options.DefaultRequestCulture = new RequestCulture("en-US");
        options.SupportedCultures = supportedCultures;
        options.SupportedUICultures = supportedCultures;
      });

      return services;
    }
  }
}
