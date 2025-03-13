using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrdersManager.Core.Domain.Entities;
using OrdersManager.Core.Domain.RepositoryContracts;
using OrdersManager.Infrastructure.DatabaseContext;

namespace OrdersManager.Infrastructure.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly ApplicationDbContext _db;

        public OrdersRepository(ApplicationDbContext _dbContext)
        {
            _db = _dbContext;
        }

        public async Task<Order> AddOrder(Order order)
        {
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            return order;
        }

        public async Task<List<Order>> GetAllOrders()
        {
            return await _db.Orders.OrderBy(x => x.OrderDate).ToListAsync();
        }

        public async Task<Order> GetOrderByOrderID(Guid id)
        {
            return await _db.Orders.FirstOrDefaultAsync(x => x.OrderId == id);
        }

        public async Task<Order> UpdateOrder(Order order)
        {
            var existingOrder = await _db.Orders.FirstOrDefaultAsync(x => x.OrderId == order.OrderId);

            if (existingOrder == null)
            {
                throw new ArgumentException($"Order with ID {order.OrderId} does not exist.");
            }

            existingOrder.OrderDate = order.OrderDate;
            existingOrder.OrderNumber = order.OrderNumber;
            existingOrder.CustomerName = order.CustomerName;
            existingOrder.TotalAmount = order.TotalAmount;

            try
            {
                await _db.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return existingOrder;
        }

        public async Task<bool> DeleteOrder(Guid id)
        {
            _db.Orders.RemoveRange(_db.Orders.Where(temp => temp.OrderId == id));

            int rowsDeleted = await _db.SaveChangesAsync();

            return rowsDeleted > 0;
        } 
    }
}
