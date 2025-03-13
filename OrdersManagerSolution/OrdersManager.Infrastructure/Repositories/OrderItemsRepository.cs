using Microsoft.EntityFrameworkCore;
using OrdersManager.Core.Domain.Entities;
using OrdersManager.Core.Domain.RepositoryContracts;
using OrdersManager.Infrastructure.DatabaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersManager.Infrastructure.Repositories
{
    public class OrderItemsRepository : IOrderItemsRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderItemsRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<OrderItem> AddOrderItem(OrderItem orderItem)
        {
            _db.OrderItems.Add(orderItem);
            await _db.SaveChangesAsync();

            return orderItem;
        }

        public async Task<bool> DeleteOrderItem(Guid itemId)
        {
            _db.OrderItems.RemoveRange(_db.OrderItems.Where(temp => temp.OrderItemId == itemId));

            int rowsDeleted = await _db.SaveChangesAsync();

            return rowsDeleted > 0;
        }

        public async Task<List<OrderItem>> GetAllOrderItems(Guid orderID)
        {
            var orderItems = await _db.OrderItems.Where(x => x.OrderId == orderID).ToListAsync();

            return orderItems;
        }

        public async Task<OrderItem?> GetOrderItemByID(Guid itemID)
        {
            return await _db.OrderItems.FirstOrDefaultAsync(x => x.OrderItemId == itemID);
        }

        public async Task<OrderItem> UpdateOrderItem(OrderItem orderItem)
        {
            var existingOrderItem = await _db.OrderItems.FindAsync(orderItem.OrderItemId);
            if (existingOrderItem == null)
            {
                throw new ArgumentException($"Order item with ID {orderItem.OrderItemId} does not exist.");
            }

            existingOrderItem.OrderId = orderItem.OrderId;
            existingOrderItem.ProductName = orderItem.ProductName;
            existingOrderItem.Quantity = orderItem.Quantity;
            existingOrderItem.UnitPrice = orderItem.UnitPrice;
            existingOrderItem.TotalPrice = orderItem.TotalPrice;

            await _db.SaveChangesAsync();

            return existingOrderItem;
        }
    }
}
