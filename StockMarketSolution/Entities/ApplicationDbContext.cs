using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<BuyOrder> BuyOrders { get; set; }
        public virtual DbSet<SellOrder> SellOrders { get; set; }

        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BuyOrder>().ToTable("BuyOrders");
            modelBuilder.Entity<SellOrder>().ToTable("SellOrders");


        }

    }
}
