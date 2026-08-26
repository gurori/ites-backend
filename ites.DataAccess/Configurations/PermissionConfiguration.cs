using ites.Core.Entities;
using ites.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ites.DataAccess.Configurations;

public sealed class PermissionConfiguration : IEntityTypeConfiguration<PermissionEntity>
{
    public void Configure(EntityTypeBuilder<PermissionEntity> builder)
    {
        builder.HasKey(p => p.Id);

        var permissions = Enum.GetValues<Permission>()
            .Select(p => new PermissionEntity { Id = (int)p, Name = p.ToString() });

        builder.HasData(permissions);
    }
}
