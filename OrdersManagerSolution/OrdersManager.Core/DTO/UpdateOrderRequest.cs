using OrdersManager.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersManager.Core.DTO
{
    public class UpdateOrderRequest
    {
        public Guid OrderId { get; set; }
        public string? OrderNumber { get; set; }
        public string? CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        public Order ToOrder()
        {
            return new Order()
            {
                OrderId = OrderId,
                OrderNumber = OrderNumber,
                CustomerName = CustomerName,
                OrderDate = OrderDate,
                TotalAmount = TotalAmount
            };
        }
    }
}
