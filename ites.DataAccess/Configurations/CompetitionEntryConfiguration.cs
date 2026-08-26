using ites.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ites.DataAccess.Configurations;

public sealed class CompetitionEntryConfiguration : IEntityTypeConfiguration<CompetitionEntry>
{
    public void Configure(EntityTypeBuilder<CompetitionEntry> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.CoverLetter).HasMaxLength(2000);

        builder
            .HasOne(r => r.Competition)
            .WithMany()
            .HasForeignKey(r => r.CompetitionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(r => r.User)
            .WithMany(u => u.CompetitionEntries)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
