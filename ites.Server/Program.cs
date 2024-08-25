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

services.AddSwaggerGen();

services.AddCors(option =>
{
    option.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(environment.IsDevelopment() 
            ? "https://localhost:3000" 
            : "https://ites.vercel.app");
        policy.AllowCredentials();
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
    });
});

services.AddMvc();

services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<IRoleRepository, RoleRepository>();
services.AddScoped<IApplicationsRepository, ApplicationsRepository>();
services.AddScoped<ICompetitionsRepository, CompetitionsRepository>();
services.AddScoped<IOrdersRepository, OrdersRepository>();
services.AddScoped<ITeamRepository, TeamRepository>();

services.AddScoped<IApplicationsService, ApplicationsService>();
services.AddScoped<IUserService, UserService>();
services.AddScoped<IPermissionService, PermissionService>();
services.AddScoped<ICompetitionsService, CompetitionsService>();
services.AddScoped<IOrdersService, OrdersService>();
services.AddScoped<ITeamService, TeamService>();
services.AddScoped<IUserProfileService, UserProfileService>();

services.AddScoped<IJwtProvider, JwtProvider>();
services.AddScoped<IPasswordHasher, PasswordHasher>();

services.AddAutoMapper(
    typeof(UserAutoMapperProfile),
    typeof(CompetitionAutoMapperProfile),
    typeof(ApplicationAutoMapperProfile),
    typeof(OrderAutoMapperProfile),
    typeof(TeamAutoMapperProfile));

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

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseRouting();

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
