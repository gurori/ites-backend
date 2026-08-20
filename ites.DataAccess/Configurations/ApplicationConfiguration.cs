using ites.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ites.DataAccess.Configurations
{
    public partial class ApplicationConfiguration : IEntityTypeConfiguration<RequestEntity>
    {
        public void Configure(EntityTypeBuilder<RequestEntity> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.From).HasMaxLength(36);

            builder.Property(a => a.For).HasMaxLength(36);
        }
    }
}
