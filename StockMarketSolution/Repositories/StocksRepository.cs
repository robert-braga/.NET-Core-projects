using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace Repositories
{
    /// <summary>
    /// Repository implementation for managing stocks into the SQL Server data store
    /// </summary>
    public class StocksRepository : IStocksRepository
    {
        private readonly ApplicationDbContext _db;

        public StocksRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<BuyOrder> CreateBuyOrder(BuyOrder buyOrder)
        {
            _db.BuyOrders.Add(buyOrder);
            await _db.SaveChangesAsync();

            return buyOrder;
        }

        public async Task<SellOrder> CreateSellOrder(SellOrder sellOrder)
        {
            _db.SellOrders.Add(sellOrder);
            await _db.SaveChangesAsync();

            return sellOrder;
        }

        public async Task<List<BuyOrder>> GetAllBuyOrders()
        {
            return await _db.BuyOrders
                    .OrderByDescending(temp => temp.DateAndTimeOfOrder)
                    .ToListAsync();
        }

        public async Task<List<SellOrder>> GetAllSellOrders()
        {
            return await _db.SellOrders
                    .OrderByDescending(temp => temp.DateAndTimeOfOrder)
                    .ToListAsync();
        }
    }
}
