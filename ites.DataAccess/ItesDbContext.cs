using ites.DataAccess.Configurations;
using ites.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ites.DataAccess
{
    public class ItesDbContext(
        DbContextOptions<ItesDbContext> options,
        IOptions<AuthorizationOptions> authOptions) 
            : DbContext(options)
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<CompetitionEntity> Competitions { get; set; }
        public DbSet<ApplicationEntity> Applications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ItesDbContext).Assembly);

            modelBuilder.ApplyConfiguration(new RolePermissionConfiguration(authOptions.Value));
        }
    }
}