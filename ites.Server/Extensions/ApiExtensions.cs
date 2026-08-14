using ites.Infrastructure.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace ites.Server.Extensions
{
    public static class ApiExtensions
    {
        public static void AddAuthentication(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();
            var scheme = JwtBearerDefaults.AuthenticationScheme;

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = scheme;
                    options.DefaultSignInScheme = scheme;
                    options.DefaultChallengeScheme = scheme;
                })
                .AddJwtBearer(
                    scheme,
                    options =>
                    {
                        options.RequireHttpsMetadata = true;
                        options.SaveToken = true;

                        options.TokenValidationParameters =
                            JwtParameters.GetTokenValidationParameters(jwtOptions!);

                        options.Events.OnMessageReceived = context =>
                        {
                            var token = context.Request.Cookies["auth"];

                            if (token is not null)
                            {
                                context.Token = token;
                            }

                            return Task.CompletedTask;
                        };
                    }
                );

            services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddSingleton<
                IAuthorizationPolicyProvider,
                PermissionAuthorizationPolicyProvider
            >();
            services.AddAuthorization();
        }
    }
}
