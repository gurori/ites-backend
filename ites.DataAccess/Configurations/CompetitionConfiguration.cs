using ites.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ites.DataAccess.Configurations;

public sealed class CompetitionConfiguration : IEntityTypeConfiguration<Competition>
{
    public void Configure(EntityTypeBuilder<Competition> builder)
    {
        builder.HasKey(c => c.Id);

        builder
            .HasMany(c => c.Members)
            .WithMany(u => u.ParticipatedCompetitions)
            .UsingEntity(j => j.ToTable("CompetitionMembers"));

        builder
            .HasMany(c => c.Organizers)
            .WithMany(u => u.OrganizedCompetitions)
            .UsingEntity(j => j.ToTable("CompetitionOrganizers"));
    }
}
