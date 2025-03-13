using FluentAssertions;
using Moq;
using OrdersManager.Core.Domain.Entities;
using OrdersManager.Core.Domain.RepositoryContracts;
using OrdersManager.Core.ServiceContracts;
using OrdersManager.Core.Services;

namespace OrdersManager.ServiceTests
{
    public class OrdersServiceTests
    {
        private readonly IOrdersService _ordersService;
        private readonly IOrdersRepository _ordersRepository;

        private readonly Mock<IOrdersRepository> _ordersRepositoryMock;

        public OrdersServiceTests()
        {
            _ordersRepositoryMock = new Mock<IOrdersRepository>();

            _ordersRepository = _ordersRepositoryMock.Object;

            _ordersService = new OrdersService(_ordersRepository);
        }

        [Fact]
        public async Task GetAllOrders_ShouldReturnOrdersList()
        {
            List<Order> expectedOrders = new List<Order>()
            {
                new Order { OrderId = Guid.Parse("F4816224-70D6-4491-AC52-34F298ACE16F"), OrderNumber = "Order_2025_1", CustomerName = "John Doe", OrderDate = new DateTime(2025, 02, 06, 08, 00, 00), TotalAmount = 66.5m },
                new Order { OrderId = Guid.Parse("735886C0-FAF3-49CA-9776-8A20B756F1CB"), OrderNumber = "Order_2025_2", CustomerName = "Jane Smith", OrderDate = new DateTime(2025, 02, 07, 08, 00, 00), TotalAmount = 225.8m }
            };

            _ordersRepositoryMock
                .Setup(temp => temp.GetAllOrders())
                .ReturnsAsync(expectedOrders);

            var actualOrders = await _ordersService.GetAllOrders();

            actualOrders.Should().BeEquivalentTo(expectedOrders);
        }

        //TODO:add the other tests
    }
}