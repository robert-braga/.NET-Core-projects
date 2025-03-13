using OrdersManager.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersManager.Core.ServiceContracts
{
    public interface IOrderItemsService
    {
        Task<List<OrderItemResponse>> GetAllOrderItems(Guid orderID);

        Task<OrderItemResponse?> GetOrderItemByID(Guid itemID);

        Task<OrderItemResponse> AddOrderItem(AddOrderItemRequest orderItem);

        Task<OrderItemResponse> UpdateOrderItem(UpdateOrderItemRequest orderItem);

        Task<bool> DeleteOrderItem(Guid itemID);
    }
}
