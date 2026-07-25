using ites.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ites.DataAccess.Configurations
{
    public partial class ApplicationConfiguration
        : IEntityTypeConfiguration<ApplicationEntity>
    {
        public void Configure(EntityTypeBuilder<ApplicationEntity> builder)
        {
            builder.HasKey(a => a.Id);

            builder
                .Property(a => a.From)
                .HasMaxLength(36);

            builder
                .Property(a => a.For)
                .HasMaxLength(36);
        }
    }
}
