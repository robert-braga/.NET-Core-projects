using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class StockMarketDbContext : DbContext
    {
        public StockMarketDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<BuyOrder> BuyOrders { get; set; }
        public DbSet<SellOrder> SellOrders { get; set; }

        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BuyOrder>().ToTable("BuyOrders");
            modelBuilder.Entity<SellOrder>().ToTable("SellOrders");


        }

    }
}
