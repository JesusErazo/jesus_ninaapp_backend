using NinaApp.API.Extensions;
using NinaApp.API.Middlewares;
using NinaApp.Core;
using NinaApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLocalization();
builder.Services.AddApiLocalization();

builder.Services.AddCore();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseRequestLocalization();

app.UseExceptionHandlingMiddleware();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
  options.SwaggerEndpoint("/swagger/v1/swagger.json", "Nina App v1");
  options.RoutePrefix = string.Empty;
});

app.UseRouting();

app.MapControllers();

app.Run();
