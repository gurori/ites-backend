using ites.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ites.DataAccess.Configurations;

public sealed class OrderBidConfiguration : IEntityTypeConfiguration<OrderBid>
{
    public void Configure(EntityTypeBuilder<OrderBid> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.CoverLetter).HasMaxLength(2000);
        builder.Property(b => b.ProposedPrice).HasColumnType("decimal(18,2)");

        builder
            .HasOne(b => b.Order)
            .WithMany(o => o.Bids)
            .HasForeignKey(b => b.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(b => b.User)
            .WithMany(u => u.OrderBids)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
