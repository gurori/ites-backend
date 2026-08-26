using ites.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ites.DataAccess.Configurations;

public sealed class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name).HasMaxLength(200).IsRequired();
        builder.Property(t => t.Description).HasMaxLength(2000);

        builder
            .HasOne(t => t.Admin)
            .WithMany()
            .HasForeignKey(t => t.AdminId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
