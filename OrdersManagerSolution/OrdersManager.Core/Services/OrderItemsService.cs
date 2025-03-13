using OrdersManager.Core.Domain.Entities;
using OrdersManager.Core.Domain.RepositoryContracts;
using OrdersManager.Core.DTO;
using OrdersManager.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersManager.Core.Services
{
    public class OrderItemsService : IOrderItemsService
    {
        private readonly IOrderItemsRepository _orderItemsRepository;

        public OrderItemsService(IOrderItemsRepository orderItemsRepository)
        {
            _orderItemsRepository = orderItemsRepository;
        }

        public async Task<OrderItemResponse> AddOrderItem(AddOrderItemRequest orderItem)
        {
            OrderItem item = new OrderItem()
            {
                OrderId = orderItem.OrderId,
                ProductName = orderItem.ProductName,
                Quantity = orderItem.Quantity,
                TotalPrice = orderItem.TotalPrice <= 0 ? orderItem.UnitPrice * orderItem.Quantity : orderItem.TotalPrice,
                UnitPrice = orderItem.UnitPrice
            };

            item.OrderItemId = Guid.NewGuid();

            var addedOrderItem = await _orderItemsRepository.AddOrderItem(item);

            return addedOrderItem.ToOrderItemResponse();
        }

        public async Task<bool> DeleteOrderItem(Guid itemID)
        {
            return await _orderItemsRepository.DeleteOrderItem(itemID);
        }

        public async Task<List<OrderItemResponse>> GetAllOrderItems(Guid orderID)
        {
            var orderItems = await _orderItemsRepository.GetAllOrderItems(orderID);

            return orderItems.ToOrderItemResponseList();
        }

        public async Task<OrderItemResponse?> GetOrderItemByID(Guid itemID)
        {
            var orderItem = await _orderItemsRepository.GetOrderItemByID(itemID);

            if (orderItem == null)
                return null;

            return orderItem.ToOrderItemResponse();
        }

        public async Task<OrderItemResponse> UpdateOrderItem(UpdateOrderItemRequest orderItem)
        {
            var updatedOrderItem = await _orderItemsRepository.UpdateOrderItem(orderItem.ToOrderItem());

            return updatedOrderItem.ToOrderItemResponse();
        }
    }
}
