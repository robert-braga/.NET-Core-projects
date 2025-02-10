using AutoFixture;
using Entities;
using EntityFrameworkCoreMock;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using System;
using Xunit.Abstractions;

namespace Tests.ServiceTests
{
    public class StocksServiceTest
    {
        private readonly IStocksService _stocksService;
        private readonly ITestOutputHelper _outputHelper;
        private readonly IFixture _fixture;

        private readonly Mock<IStocksRepository> _stocksRepositoryMock;
        private readonly IStocksRepository _stocksRepository;

        public StocksServiceTest(ITestOutputHelper testOutputHelper)
        {
            _outputHelper = testOutputHelper;
            _fixture = new Fixture();

            _stocksRepositoryMock = new Mock<IStocksRepository>();
            _stocksRepository = _stocksRepositoryMock.Object;

            //DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>(
            //    new DbContextOptionsBuilder<ApplicationDbContext>().Options
            //);

            //ApplicationDbContext applicationDbContext = dbContextMock.Object;

            //var buyOrdersInitialData = new List<BuyOrder>();
            //var sellOrdersInitialData = new List<SellOrder>();

            //dbContextMock.CreateDbSetMock(t => t.BuyOrders, buyOrdersInitialData);
            //dbContextMock.CreateDbSetMock(t => t.SellOrders, sellOrdersInitialData);

            _stocksService = new StocksService(_stocksRepository);
        }



        #region CreateBuyOrder
        //When you supply BuyOrderRequest as null, it should throw ArgumentNullException
        [Fact]
        public async Task CreateBuyOrder_BuyOrderRequestIsNull_ToBeArgumentNullException()
        {
            // Arrange
            BuyOrderRequest? buyOrderRequest = null;

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        //When you supply buyOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Theory]
        [InlineData(0)]
        public async Task CreateBuyOrder_QuantityIsZero_ToBeArgumentException(int quantity)
        {
            // Arrange
            BuyOrderRequest buyOrderRequest = _fixture
                .Build<BuyOrderRequest>()
                .With(x => x.Quantity, quantity)
                .Create();

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        // When you supply buyOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_QuantityIsOverMaximum_ToBeArgumentException()
        {
            // Arrange
            BuyOrderRequest buyOrderRequest = _fixture
                .Build<BuyOrderRequest>()
                .With(x => x.Quantity, 100001)
                .Create();

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When you supply buyOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_PriceIsZero_ToBeArgumentException()
        {
            // Arrange
            BuyOrderRequest buyOrderRequest = _fixture
               .Build<BuyOrderRequest>()
               .With(x => x.Price, 0)
               .Create();

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        // When you supply buyOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_PriceIsOverMaximum_ToBeArgumentException()
        {
            // Arrange
            BuyOrderRequest buyOrderRequest = _fixture
                .Build<BuyOrderRequest>()
                .With(x => x.Price, 10001)
                .Create();

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_StockSymbolIsNull_ToBeArgumentException()
        {
            // Arrange
            BuyOrderRequest buyOrderRequest = _fixture
               .Build<BuyOrderRequest>()
               .With(x => x.StockSymbol, null as string)
               .Create();

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            };

            //Assert
            var exception = await action.Should().ThrowAsync<ArgumentException>();

            _outputHelper.WriteLine($"Exception message: {exception?.Which.Message}");
        }

        //When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification, it should be equal or newer date than 2000-01-01), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_InvalidDate_ToBeArgumentException()
        {
            // Arrange
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest()
            {
                StockSymbol = "AAPL",
                StockName = "Apple Inc.",
                DateAndTimeOfOrder = DateTime.Parse("12/31/1999"),
                Quantity = 2,
                Price = 12
            };

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            };

            //Assert
            var exception = await action.Should().ThrowAsync<ArgumentException>();

            _outputHelper.WriteLine($"Exception message: {exception?.Which.Message}");
        }

        //If you supply all valid values, it should be successful and return an object of BuyOrderResponse type with auto-generated BuyOrderID (guid).
        [Fact]
        public async Task CreateBuyOrder_ValidRequest_ToBeBuySuccessful()
        {
            // Arrange
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest()
            {
                StockSymbol = "AAPL",
                StockName = "Apple Inc.",
                DateAndTimeOfOrder = DateTime.Now,
                Quantity = 2,
                Price = 12
            };

            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            BuyOrderResponse buyOrderResponseExpected = buyOrder.ToBuyOrderResponse();

            //Act 
            _stocksRepositoryMock
                .Setup(t => t.CreateBuyOrder(It.IsAny<BuyOrder>()))
                .ReturnsAsync(buyOrder);

            BuyOrderResponse responseFromCreate = await _stocksService.CreateBuyOrder(buyOrderRequest);
            buyOrderResponseExpected.BuyOrderID = responseFromCreate.BuyOrderID;

            //Assert
            responseFromCreate.BuyOrderID.Should().NotBe(Guid.Empty);
            responseFromCreate.Should().Be(buyOrderResponseExpected);

            _outputHelper.WriteLine($"Buy order response object: {responseFromCreate.ToString()}");
        }
        #endregion

        #region CreateSellOrder
        //1. When you supply SellOrderRequest as null, it should throw ArgumentNullException.
        [Fact]
        public async Task CreateSellOrder_BuyOrderRequestIsNull_ToBeArgumentNullException()
        {
            // Arrange
            SellOrderRequest? sellOrderRequest = null;

            //Assert
            Func<Task> action = async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            };

            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        //When you supply sellOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public async Task CreateSellOrder_QuantityIsZero_ToBeArgumentException()
        {
            // Arrange
            SellOrderRequest sellOrderRequest = new SellOrderRequest()
            {
                StockSymbol = "AAPL",
                StockName = "Apple Inc.",
                DateAndTimeOfOrder = DateTime.Now,
                Quantity = 0,
                Price = 100
            };

            //Assert
            Func<Task> action = async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        // When you supply sellOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.
        [Fact]
        public async Task CreateSellOrder_QuantityIsOverMaximum_ToBeArgumentException()
        {
            // Arrange
            SellOrderRequest sellOrderRequest = new SellOrderRequest()
            {
                StockSymbol = "AAPL",
                StockName = "Apple Inc.",
                DateAndTimeOfOrder = DateTime.Now,
                Quantity = 100001,
                Price = 100
            };

            //Assert
            Func<Task> action = async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When you supply sellOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public async Task CreateSellOrder_PriceIsZero_ToBeArgumentException()
        {
            // Arrange
            SellOrderRequest sellOrderRequest = new SellOrderRequest()
            {
                StockSymbol = "AAPL",
                StockName = "Apple Inc.",
                DateAndTimeOfOrder = DateTime.Now,
                Quantity = 2,
                Price = 0
            };

            //Assert
            Func<Task> action = async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        // When you supply sellOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.
        [Fact]
        public async Task CreateSellOrder_PriceIsOverMaximum_ToBeArgumentException()
        {
            // Arrange
            SellOrderRequest sellOrderRequest = new SellOrderRequest()
            {
                StockSymbol = "AAPL",
                StockName = "Apple Inc.",
                DateAndTimeOfOrder = DateTime.Now,
                Quantity = 2,
                Price = 10001
            };

            //Assert
            Func<Task> action = async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException
        [Fact]
        public async Task CreateSellOrder_StockSymbolIsNull_ToBeArgumentException()
        {
            // Arrange
            SellOrderRequest sellOrderRequest = new SellOrderRequest()
            {
                StockSymbol = null,
                StockName = "Apple Inc.",
                DateAndTimeOfOrder = DateTime.Now,
                Quantity = 2,
                Price = 12
            };

            //Assert
            Func<Task> action = async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            };

            var exception = await action.Should().ThrowAsync<ArgumentException>();

            _outputHelper.WriteLine($"Exception message: {exception?.Which.Message}");
        }

        //When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification, it should be equal or newer date than 2000-01-01), it should throw ArgumentException.
        [Fact]
        public async Task CreateSellOrder_InvalidDate_ToBeArgumentException()
        {
            // Arrange
            SellOrderRequest sellOrderRequest = new SellOrderRequest()
            {
                StockSymbol = "AAPL",
                StockName = "Apple Inc.",
                DateAndTimeOfOrder = DateTime.Parse("12/31/1999"),
                Quantity = 2,
                Price = 12
            };

            //Assert
            Func<Task> action = async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            };

            var exception = await action.Should().ThrowAsync<ArgumentException>();

            _outputHelper.WriteLine($"Exception message: {exception?.Which.Message}");
        }

        // If you supply all valid values, it should be successful and return an object of SellOrderResponse type with auto-generated SellOrderID (guid).
        [Fact]
        public async void CreateSellOrder_ValidRequest_ToBeSuccessful()
        {
            // Arrange
            SellOrderRequest sellOrderRequest = new SellOrderRequest()
            {
                StockSymbol = "AAPL",
                StockName = "Apple Inc.",
                DateAndTimeOfOrder = DateTime.Now,
                Quantity = 2,
                Price = 12
            };

            //Act 
            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            SellOrderResponse sellOrderResponseExpected = sellOrder.ToSellOrderResponse();

            _stocksRepositoryMock
                .Setup(t => t.CreateSellOrder(It.IsAny<SellOrder>()))
                .ReturnsAsync(sellOrder);

            SellOrderResponse responseFromCreate = await _stocksService.CreateSellOrder(sellOrderRequest);
            sellOrderResponseExpected.SellOrderID = responseFromCreate.SellOrderID;

            //Assert
            responseFromCreate.SellOrderID.Should().NotBe(Guid.Empty);
            responseFromCreate.Should().Be(sellOrderResponseExpected);

            _outputHelper.WriteLine($"Sell order response object: {responseFromCreate.ToString()}");
        }

        #endregion

        #region GetAllBuyOrders
        //1. When you invoke this method, by default, the returned list should be empty.
        [Fact]
        public async void GetAllBuyOrders_EmptyList_ToBeEmptyList()
        {
            List<BuyOrder> buyOrders = new List<BuyOrder>();

            _stocksRepositoryMock
                .Setup(t => t.GetAllBuyOrders())
                .ReturnsAsync(buyOrders);

            List<BuyOrderResponse> buyOrdersFromGet = await _stocksService.GetBuyOrders();

            buyOrdersFromGet.Should().BeEmpty();
        }

        //2. When you first add few buy orders using CreateBuyOrder() method; and then invoke GetAllBuyOrders() method; the returned list should contain all the same buy orders.

        [Fact]
        public async void GetAllBuyOrders_AddFewOrders_ToBeSuccessful()
        {
            //Arrange 
            List<BuyOrder> buyOrders = new List<BuyOrder>()
            {
                _fixture.Build<BuyOrder>()
                        .With(t => t.Quantity, 2)
                        .With(t => t.Price, 12)
                        .Create(),
                _fixture.Build<BuyOrder>()
                        .With(t => t.Quantity, 1)
                        .With(t => t.Price, 2)
                        .Create(),
                _fixture.Build<BuyOrder>()
                        .With(t => t.Quantity, 5)
                        .With(t => t.Price, 122)
                        .Create()
            };

            List<BuyOrderResponse> buyOrdersAfterAdd = buyOrders
                .Select(x => x.ToBuyOrderResponse())
                .ToList();

            _stocksRepositoryMock
                .Setup(t => t.GetAllBuyOrders())
                .ReturnsAsync(buyOrders);

            List<BuyOrderResponse> buyOrdersFromGet = await _stocksService.GetBuyOrders();

            buyOrdersFromGet.Should().BeEquivalentTo(buyOrders);
        }
        #endregion

        #region GetAllSellOrders
        //1. When you invoke this method, by default, the returned list should be empty.
        [Fact]
        public async void GetAllSellOrders_EmptyList_ToBeEmptyList()
        {
            List<SellOrder> sellOrders = new List<SellOrder>();

            _stocksRepositoryMock
                .Setup(t => t.GetAllSellOrders())
                .ReturnsAsync(sellOrders);

            List<SellOrderResponse> sellOrdersFromGet = await _stocksService.GetSellOrders();

            sellOrdersFromGet.Should().BeEmpty();
        }

        //2. When you first add few sell orders using CreateSellOrder() method; and then invoke GetAllSellOrders() method; the returned list should contain all the same sell orders.
        [Fact]
        public async void GetAllSellOrders_AddFewOrders()
        {
            //Arrange 
            List<SellOrder> sellOrders = new List<SellOrder>()
            {
                _fixture.Build<SellOrder>()
                        .With(t => t.Quantity, 2)
                        .With(t => t.Price, 12)
                        .Create(),
                _fixture.Build<SellOrder>()
                        .With(t => t.Quantity, 1)
                        .With(t => t.Price, 2)
                        .Create(),
                _fixture.Build<SellOrder>()
                        .With(t => t.Quantity, 5)
                        .With(t => t.Price, 122)
                        .Create()
            };

            List<SellOrderResponse> sellOrdersAfterAdd = sellOrders
                .Select(x => x.ToSellOrderResponse())
                .ToList();

            _stocksRepositoryMock
                .Setup(t => t.GetAllSellOrders())
                .ReturnsAsync(sellOrders);

            List<SellOrderResponse> sellOrdersFromGet = await _stocksService.GetSellOrders();

            sellOrdersFromGet.Should().BeEquivalentTo(sellOrders);
        }
        #endregion
    }
}