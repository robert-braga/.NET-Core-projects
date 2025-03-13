using OrdersManager.Core.DTO;
using OrdersManager.Core.Domain.Entities;

namespace OrdersManager.Core.ServiceContracts
{
    /// <summary>
    /// Service for managing <see cref="Order"> entity
    /// </summary>
    public interface IOrdersService
    {
        Task<List<OrderResponse>> GetAllOrders();

        Task<OrderResponse> GetOrderByOrderID(Guid orderID);

        Task<OrderResponse> AddOrder(AddOrderRequest order);

        Task<OrderResponse> UpdateOrder(UpdateOrderRequest order);

        Task<bool> DeleteOrder(Guid id);
    }
}
