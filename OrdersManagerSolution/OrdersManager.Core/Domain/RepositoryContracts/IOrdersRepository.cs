using OrdersManager.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersManager.Core.Domain.RepositoryContracts
{
    /// <summary>
    /// Repository interface for manipulating orders in database
    /// </summary>
    public interface IOrdersRepository
    {
        /// <summary>
        /// Retrieve all the orders from database
        /// </summary>
        /// <returns></returns>
        Task<List<Order>> GetAllOrders();

        /// <summary>
        /// Retrieve an order using the given order id
        /// </summary>
        /// <param name="id">Order ID to search</param>
        /// <returns>The found order</returns>
        Task<Order> GetOrderByOrderID(Guid id);

        /// <summary>
        /// Add order in database
        /// </summary>
        /// <param name="order">Order to add</param>
        /// <returns>Order added</returns>
        Task<Order> AddOrder(Order order);

        /// <summary>
        /// Update order 
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        Task<Order> UpdateOrder(Order order);

        Task<bool> DeleteOrder(Guid id); 
    }
}
