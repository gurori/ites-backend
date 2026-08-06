using ites.Core.Enums;
using Microsoft.AspNetCore.Authorization;

namespace ites.Infrastructure.Auth
{
    public class PermissionRequirement(string permission) : IAuthorizationRequirement
    {
        public string Permission { get; } = permission;
    }
}
