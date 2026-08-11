using ites.Core.Enums;
using Microsoft.AspNetCore.Authorization;

namespace ites.Infrastructure.Auth
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement
        )
        {
            var permissionsIds = context
                .User.Claims.Where(c => c.Type == ClaimNames.Permissions)
                .Select(c => c.Value)
                .First()
                .Split(';')
                .Select(Enum.Parse<Permission>)
                .ToHashSet();

            if (permissionsIds.Contains(requirement.Permission))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
