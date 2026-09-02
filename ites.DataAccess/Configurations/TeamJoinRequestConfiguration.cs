using ites.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ites.DataAccess.Configurations;

public sealed class TeamJoinRequestConfiguration : IEntityTypeConfiguration<TeamJoinRequest>
{
    public void Configure(EntityTypeBuilder<TeamJoinRequest> builder)
    {
        builder.HasKey(j => j.Id);
        builder.Property(j => j.CoverLetter).HasMaxLength(2000);

        builder
            .HasOne(j => j.Team)
            .WithMany(t => t.JoinRequests)
            .HasForeignKey(j => j.TeamId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(j => j.User)
            .WithMany(u => u.TeamJoinRequests)
            .HasForeignKey(j => j.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
