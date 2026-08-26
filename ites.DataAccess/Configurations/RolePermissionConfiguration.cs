using ites.Core.Entities;
using ites.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ites.DataAccess.Configurations;

public sealed class RolePermissionConfiguration(AuthorizationOptions authorizationOptions)
    : IEntityTypeConfiguration<RolePermissionEntity>
{
    public void Configure(EntityTypeBuilder<RolePermissionEntity> builder)
    {
        builder.HasKey(r => new { r.RoleId, r.PermissionId });

        builder.HasData(ParseRolePermissions());
    }

    private RolePermissionEntity[] ParseRolePermissions() =>
        authorizationOptions
            .RolePermissions.SelectMany(rp =>
                rp.Permissions.Select(p => new RolePermissionEntity
                {
                    RoleId = (int)Enum.Parse<Role>(rp.Role),
                    PermissionId = (int)Enum.Parse<Permission>(p),
                })
            )
            .ToArray();
}
