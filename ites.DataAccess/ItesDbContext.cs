using ites.Core.Entities;
using ites.DataAccess.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ites.DataAccess
{
    public class ItesDbContext(
        DbContextOptions<ItesDbContext> options,
        IOptions<AuthorizationOptions> authOptions
    ) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<Competition> Competitions { get; set; }
        public DbSet<CompetitionEntry> CompetitionEntries { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderBid> OrderBids { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamJoinRequest> TeamJoinRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ItesDbContext).Assembly);

            modelBuilder.ApplyConfiguration(new RolePermissionConfiguration(authOptions.Value));
        }
    }
}
