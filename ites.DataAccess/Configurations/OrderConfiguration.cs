using ites.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ites.DataAccess.Configurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Title).HasMaxLength(250).IsRequired();
        builder.Property(o => o.Price).HasColumnType("decimal(18,2)");

        builder
            .HasOne(o => o.Client)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(o => o.Member)
            .WithMany()
            .HasForeignKey(o => o.MemberId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
