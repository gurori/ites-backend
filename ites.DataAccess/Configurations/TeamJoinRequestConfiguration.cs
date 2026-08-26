using ites.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ites.DataAccess.Configurations;

public sealed class TeamJoinRequestConfiguration : IEntityTypeConfiguration<TeamJoinRequest>
{
    public void Configure(EntityTypeBuilder<TeamJoinRequest> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.CoverLetter).HasMaxLength(2000);

        builder
            .HasOne(r => r.Team)
            .WithMany()
            .HasForeignKey(r => r.TeamId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(r => r.User)
            .WithMany(u => u.TeamJoinRequests)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
