using Entities;

namespace RepositoryContracts
{
    /// <summary>
    /// Data access logic layer (DLL) for managing stocks entities: BuyOrder and SellOrder
    /// </summary>
    public interface IStocksRepository
    {
        /// <summary>
        /// Inserts a new buy order in the data store
        /// </summary>
        /// <param name="buyOrder">Buy Order to insert into data store</param>
        /// <returns>Inserted buy order</returns>
        Task<BuyOrder> CreateBuyOrder(BuyOrder buyOrder);

        /// <summary>
        /// Inserts a new sell order in the data store
        /// </summary>
        /// <param name="sellOrder">Sell order to insert into data store</param>
        /// <returns>Inserted sell order</returns>
        Task<SellOrder> CreateSellOrder(SellOrder sellOrder);

        /// <summary>
        /// Retrieves all buy orders from the data store
        /// </summary>
        /// <returns>Buy orders from data store</returns>
        Task<List<BuyOrder>> GetAllBuyOrders();

        /// <summary>
        /// Retrieves all sell orders from the data store
        /// </summary>
        /// <returns>Sell orders from data store</returns>
        Task<List<SellOrder>> GetAllSellOrders();
    }
}
