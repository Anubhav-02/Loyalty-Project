using Microsoft.EntityFrameworkCore;
using LoyaltyApi.Models;

namespace LoyaltyApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Member> Members => Set<Member>();
        public DbSet<PointsTransaction> PointsTransactions => Set<PointsTransaction>();
        public DbSet<CouponRedemption> CouponRedemptions => Set<CouponRedemption>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Member>()
                .HasIndex(m => m.MobileNumber)
                .IsUnique();
        }
    }
}
