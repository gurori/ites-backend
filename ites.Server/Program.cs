using ites.DataAccess;
using ites.Infrastructure.Auth;
using ites.Server.Extensions;
using ites.Server.Filters;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;
var environment = builder.Environment;

services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
services.Configure<AuthorizationOptions>(configuration.GetSection(nameof(AuthorizationOptions)));

services.AddSwaggerGen();

var corsOrigins =
    configuration.GetSection("Cors:Origins").Get<string[]>()
    ?? throw new InvalidOperationException("Configuration string 'Cors:Origins' not found.");

services.AddCors(option =>
{
    option.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(corsOrigins);
        policy.AllowCredentials();
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
    });
});

services.AddRouting(options => 
{
    options.LowercaseUrls = true;
});

services.AddRepositories();

services.AddApplicationServices();

services.AddAuthentication(configuration);

services.AddControllers(options =>
{
    options.Filters.Add<ApiExceptionFilter>();
});

services.AddDbContext<ItesDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString(nameof(ItesDbContext)))
);

var app = builder.Build();

using var scope = app.Services.CreateScope();
await using var dbContext = scope.ServiceProvider.GetRequiredService<ItesDbContext>();
await dbContext.Database.EnsureCreatedAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseRouting();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
