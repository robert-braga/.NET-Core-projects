using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrdersManager.Core.Domain.Entities;

namespace OrdersManager.Core.DTO
{
    public class OrderResponse
    {
        public Guid OrderId { get; set; }

        public string? OrderNumber { get; set; }

        public string? CustomerName { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }
    }

    /// <summary>
    /// Provides extension methods for <see cref="Order"/> entity
    /// </summary>
    public static class OrderExtensions
    {
        public static OrderResponse ToOrderResponse(this Order order)
        {
            return new OrderResponse()
            {
                OrderId = order.OrderId,
                OrderNumber = order.OrderNumber,
                CustomerName = order.CustomerName,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount
            };
        }

        public static List<OrderResponse> ToOrderResponseList(this List<Order> orders)
        {
            return orders
                .Select(x => new OrderResponse()
            {
                OrderId = x.OrderId,
                OrderNumber = x.OrderNumber,
                CustomerName = x.CustomerName,
                OrderDate = x.OrderDate,
                TotalAmount = x.TotalAmount
            })
                .ToList();
        }
    }
}
