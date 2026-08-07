using ites.Application.Interfaces.Auth;
using ites.Application.Interfaces.Services;
using ites.Application.Services;
using ites.Core.Interfaces.Repositories;
using ites.DataAccess.Repositories;
using ites.Infrastructure.Auth;
using ites.Infrastructure.Files;

namespace ites.Server.Extensions;

public static class DIExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IApplicationsRepository, ApplicationsRepository>();
        services.AddScoped<ICompetitionsRepository, CompetitionsRepository>();
        services.AddScoped<IOrdersRepository, OrdersRepository>();
        services.AddScoped<ITeamRepository, TeamRepository>();

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IApplicationsService, ApplicationsService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<ICompetitionsService, CompetitionsService>();
        services.AddScoped<IOrdersService, OrdersService>();
        services.AddScoped<ITeamService, TeamService>();
        services.AddScoped<IUserProfileService, UserProfileService>();
        services.AddScoped<IModersService, ModersService>();

        services.AddScoped<IFileService, FileService>();

        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        return services;
    }
}
