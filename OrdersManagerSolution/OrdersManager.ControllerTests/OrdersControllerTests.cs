using Microsoft.AspNetCore.Mvc;
using Moq;
using OrdersManager.Core.Domain.Entities;
using OrdersManager.Core.Domain.RepositoryContracts;
using OrdersManager.Core.DTO;
using OrdersManager.Core.ServiceContracts;
using OrdersManager.Core.Services;
using OrdersManager.WebAPI.Controllers.v1;

namespace OrdersManager.ControllerTests
{
    public class OrdersControllerTests
    {
        private readonly IOrdersService _ordersService;

        private readonly IOrdersRepository _ordersRepository;
        private readonly Mock<IOrdersRepository> _ordersRepositoryMock;

        public OrdersControllerTests()
        {
            _ordersRepositoryMock = new Mock<IOrdersRepository>();

            _ordersRepository = _ordersRepositoryMock.Object;

            _ordersService = new OrdersService(_ordersRepository);
        }


        [Fact]
        public async void Test1()
        {
            OrdersController ordersController = new OrdersController(_ordersService);

            List<Order> orders = new List<Order>()
            {
                new Order { OrderId = Guid.Parse("F4816224-70D6-4491-AC52-34F298ACE16F"), OrderNumber = "Order_2025_1", CustomerName = "John Doe", OrderDate = new DateTime(2025, 02, 06, 08, 00, 00), TotalAmount = 66.5m },
                new Order { OrderId = Guid.Parse("735886C0-FAF3-49CA-9776-8A20B756F1CB"), OrderNumber = "Order_2025_2", CustomerName = "Jane Smith", OrderDate = new DateTime(2025, 02, 07, 08, 00, 00), TotalAmount = 225.8m }
            };

            _ordersRepositoryMock
                .Setup(temp => temp.GetAllOrders())
                .ReturnsAsync(orders);

            var result = await ordersController.GetOrders();

            var okResult = Assert.IsType<ActionResult<List<OrderResponse>>>(result);
            var returnedOrders = Assert.IsAssignableFrom<List<OrderResponse>>(okResult.Value);
            Assert.Equal(2, orders.Count);

        }
    }
}