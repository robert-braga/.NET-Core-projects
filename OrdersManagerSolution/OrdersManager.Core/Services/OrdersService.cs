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
    public class OrdersService : IOrdersService
    {

        private readonly IOrdersRepository _ordersRepository;

        public OrdersService(IOrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;
        }

        public async Task<OrderResponse> AddOrder(AddOrderRequest order)
        {
            Order orderToAdd = new Order()
            {
                OrderNumber = order.OrderNumber,
                CustomerName = order.CustomerName,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount
            };

            orderToAdd.OrderId = Guid.NewGuid();

            var addedOrder = await _ordersRepository.AddOrder(orderToAdd);

            return addedOrder.ToOrderResponse();
        }

        public async Task<bool> DeleteOrder(Guid id)
        {
            return await _ordersRepository.DeleteOrder(id);
        }

        public async Task<List<OrderResponse>> GetAllOrders()
        {
            var orders = await _ordersRepository.GetAllOrders();

            return orders.ToOrderResponseList();
        }

        public async Task<OrderResponse> GetOrderByOrderID(Guid orderID)
        {
            var order = await _ordersRepository.GetOrderByOrderID(orderID);
            return order.ToOrderResponse();
        }

        public async Task<OrderResponse> UpdateOrder(UpdateOrderRequest order)
        {
            var updatedOrder = await _ordersRepository.UpdateOrder(order.ToOrder());

            return updatedOrder.ToOrderResponse();
        }
    }
}
