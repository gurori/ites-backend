using ites.Application.Interfaces.Auth;
using ites.Application.Interfaces.Repositories;
using ites.Application.Interfaces.Services;
using ites.Application.Services;
using ites.DataAccess;
using ites.DataAccess.Repositories;
using ites.Infastructure.Auth;
using ites.Infastructure.Mapping;
using ites.Server.Extensions;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
var environment = builder.Environment;

services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
services.Configure<AuthorizationOptions>(configuration.GetSection(nameof(AuthorizationOptions)));

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddSwaggerGen();

services.AddCors(option =>
{
    option.AddDefaultPolicy(policy =>
    {
        //you should change origins after developming :3
        policy.WithOrigins("http://localhost:3000");
        //(environment.IsDevelopment() ? "http://localhost:3000" : "client.com");
        policy.AllowCredentials();
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
    });
});

services.AddMvc();

services.AddScoped<IUserRepository, UserRepository>();

services.AddScoped<IUserService, UserService>();
services.AddScoped<IPermissionService, PermissionService>();

services.AddScoped<IJwtProvider, JwtProvider>();
services.AddScoped<IPasswordHasher, PasswordHasher>();

services.AddAutoMapper(typeof(UserAutoMapperProfile));

services.AddAuthentication(configuration);

services.AddControllers();

services.AddDbContext<ItesDbContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString(nameof(ItesDbContext))));

var app = builder.Build();

using var scope = app.Services.CreateScope();
await using var dbContext = scope.ServiceProvider.GetRequiredService<ItesDbContext>();
await dbContext.Database.EnsureCreatedAsync();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.None,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always,
});

app.MapControllers();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
