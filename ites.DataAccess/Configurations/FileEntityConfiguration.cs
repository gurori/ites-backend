using ites.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ites.DataAccess.Configurations;

public sealed class FileEntityConfiguration : IEntityTypeConfiguration<FileEntity>
{
    public void Configure(EntityTypeBuilder<FileEntity> builder)
    {
        builder.HasKey(f => f.Id);

        builder.Property(f => f.Directory).HasMaxLength(100).IsRequired();
        builder.Property(f => f.FileName).HasMaxLength(255).IsRequired();
        builder.Property(f => f.ContentType).HasMaxLength(100).IsRequired();

        builder
            .HasOne(f => f.UploadedBy)
            .WithMany(u => u.Files)
            .HasForeignKey(f => f.UploadedById)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
