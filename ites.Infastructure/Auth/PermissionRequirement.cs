using ites.Core.Enums;
using Microsoft.AspNetCore.Authorization;

namespace ites.Infastructure.Auth
{
    public class PermissionRequirement(string permission)
                : IAuthorizationRequirement
    {
        public string Permission { get; } = permission;
    }
}
