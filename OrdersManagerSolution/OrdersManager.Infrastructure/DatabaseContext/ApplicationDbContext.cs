using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OrdersManager.Core.Domain.Entities;
using OrdersManager.Core.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersManager.Infrastructure.DatabaseContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //map entities to tables
            modelBuilder.Entity<Order>().ToTable("Orders");
            modelBuilder.Entity<OrderItem>().ToTable("OrderItems");

            // seed data
            modelBuilder.Entity<Order>().HasData(
                new Order { OrderId = Guid.Parse("F4816224-70D6-4491-AC52-34F298ACE16F"), OrderNumber = "Order_2025_1", CustomerName = "John Doe", OrderDate = new DateTime(2025, 02, 06, 08, 00, 00), TotalAmount = 66.5m },
                new Order { OrderId = Guid.Parse("735886C0-FAF3-49CA-9776-8A20B756F1CB"), OrderNumber = "Order_2025_2", CustomerName = "Jane Smith", OrderDate = new DateTime(2025, 02, 07, 08, 00, 00), TotalAmount = 225.8m }
            );

            modelBuilder.Entity<OrderItem>().HasData(
                new OrderItem { OrderItemId = Guid.Parse("D20882DF-7FCA-4EE8-88BB-37D2FC75E63F"), OrderId = Guid.Parse("F4816224-70D6-4491-AC52-34F298ACE16F"), ProductName = "Product A", Quantity = 2, UnitPrice = 10.00m, TotalPrice = 20.00m },
                new OrderItem { OrderItemId = Guid.Parse("2E27B6A4-469D-4D7F-8B8B-54AF129675FD"), OrderId = Guid.Parse("F4816224-70D6-4491-AC52-34F298ACE16F"), ProductName = "Product B", Quantity = 3, UnitPrice = 15.50m, TotalPrice = 46.50m },
                new OrderItem { OrderItemId = Guid.Parse("24D71AC2-0A9C-4914-9FD3-13BC25D42694"), OrderId = Guid.Parse("735886C0-FAF3-49CA-9776-8A20B756F1CB"), ProductName = "Product C", Quantity = 7, UnitPrice = 25.40m, TotalPrice = 25.00m },
                new OrderItem { OrderItemId = Guid.Parse("AC90B8BC-349D-43FD-87A6-6A7ED8057697"), OrderId = Guid.Parse("735886C0-FAF3-49CA-9776-8A20B756F1CB"), ProductName = "Product D", Quantity = 4, UnitPrice = 12.00m, TotalPrice = 25.00m }
            );
        }
    }
}
