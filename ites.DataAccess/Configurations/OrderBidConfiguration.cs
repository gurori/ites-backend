using ites.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ites.DataAccess.Configurations;

public sealed class OrderBidConfiguration : IEntityTypeConfiguration<OrderBid>
{
    public void Configure(EntityTypeBuilder<OrderBid> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.CoverLetter).HasMaxLength(2000);
        builder.Property(r => r.ProposedPrice).HasColumnType("decimal(18,2)");

        builder
            .HasOne(r => r.Order)
            .WithMany()
            .HasForeignKey(r => r.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(r => r.User)
            .WithMany(u => u.OrderBids)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
