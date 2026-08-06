using ites.Core.Enums;
using Microsoft.AspNetCore.Authorization;

namespace ites.Infrastructure.Auth
{
    public sealed class HasPermissionAttribute(Permission permission)
        : AuthorizeAttribute(policy: permission.ToString()) { }
}
