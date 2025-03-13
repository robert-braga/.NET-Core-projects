using OrdersManager.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersManager.Core.Domain.RepositoryContracts
{
    public interface IOrderItemsRepository
    {
        Task<List<OrderItem>> GetAllOrderItems(Guid orderID);
        Task<OrderItem?> GetOrderItemByID(Guid itemID);
        Task<OrderItem> AddOrderItem(OrderItem orderItem);
        Task<OrderItem> UpdateOrderItem(OrderItem orderItem);
        Task<bool> DeleteOrderItem(Guid itemId);

    }
}
