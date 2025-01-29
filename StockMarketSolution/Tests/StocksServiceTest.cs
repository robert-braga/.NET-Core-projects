using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using Xunit.Abstractions;

namespace Tests
{
    public class StocksServiceTest
    {
        private readonly IStocksService _stocksService;
        private readonly ITestOutputHelper _outputHelper;

        public StocksServiceTest(ITestOutputHelper testOutputHelper)
        {
            _stocksService = new StocksService(new StockMarketDbContext(new DbContextOptionsBuilder<StockMarketDbContext>().Options));
            _outputHelper = testOutputHelper;
        }



        #region CreateBuyOrder
        //When you supply BuyOrderRequest as null, it should throw ArgumentNullException
        [Fact]
        public void CreateBuyOrder_BuyOrderRequestIsNull()
        {
            // Arrange
            BuyOrderRequest? buyOrderRequest = null;

            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                //Act
                _stocksService.CreateBuyOrder(buyOrderRequest)
            );
        }

        //When you supply buyOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Theory]
        [InlineData(0)]
        public void CreateBuyOrder_QuantityIsZero(uint quantity)
        {
            // Arrange
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest()
            {
                StockSymbol = "AAPL",
                StockName = "Apple Inc.",
                DateAndTimeOfOrder = DateTime.Now,
                Quantity = quantity,
                Price = 100
            };

            //Assert
            Assert.ThrowsAsync<ArgumentException>(() =>
                //Act
                _stocksService.CreateBuyOrder(buyOrderRequest)
            );
        }

        // When you supply buyOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_QuantityIsOverMaximum()
        {
            // Arrange
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest()
            {
                StockSymbol = "AAPL",
                StockName = "Apple Inc.",
                DateAndTimeOfOrder = DateTime.Now,
                Quantity = 100001,
                Price = 100
            };

            //Assert
            Assert.ThrowsAsync<ArgumentException>(() =>
                //Act
                _stocksService.CreateBuyOrder(buyOrderRequest)
            );
        }

        //When you supply buyOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_PriceIsZero()
        {
            // Arrange
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest()
            {
                StockSymbol = "AAPL",
                StockName = "Apple Inc.",
                DateAndTimeOfOrder = DateTime.Now,
                Quantity = 2,
                Price = 0
            };

            //Assert
            Assert.ThrowsAsync<ArgumentException>(() =>
                //Act
                _stocksService.CreateBuyOrder(buyOrderRequest)
            );
        }

        // When you supply buyOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_PriceIsOverMaximum()
        {
            // Arrange
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest()
            {
                StockSymbol = "AAPL",
                StockName = "Apple Inc.",
                DateAndTimeOfOrder = DateTime.Now,
                Quantity = 2,
                Price = 10001
            };

            //Assert
            Assert.ThrowsAsync<ArgumentException>(() =>
                //Act
                _stocksService.CreateBuyOrder(buyOrderRequest)
            );
        }

        //When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_StockSymbolIsNull()
        {
            // Arrange
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest()
            {
                StockSymbol = null,
                StockName = "Apple Inc.",
                DateAndTimeOfOrder = DateTime.Now,
                Quantity = 2,
                Price = 12
            };

            //Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(() =>
                //Act
                _stocksService.CreateBuyOrder(buyOrderRequest)
            );

            _outputHelper.WriteLine($"Exception message: {exception?.Result.Message}");
        }

        //When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification, it should be equal or newer date than 2000-01-01), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_InvalidDate()
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

            //Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(() =>
                //Act
                _stocksService.CreateBuyOrder(buyOrderRequest)
            );

            _outputHelper.WriteLine($"Exception message: {exception?.Result.Message}");
        }

        //If you supply all valid values, it should be successful and return an object of BuyOrderResponse type with auto-generated BuyOrderID (guid).
        [Fact]
        public async void CreateBuyOrder_ValidRequest()
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

            //Act 
            BuyOrderResponse responseFromCreate = await _stocksService.CreateBuyOrder(buyOrderRequest);

            //Assert
            Assert.NotEqual(responseFromCreate.BuyOrderID, Guid.Empty);

            _outputHelper.WriteLine($"Buy order response object: {responseFromCreate.ToString()}");
        }
        #endregion

        #region CreateSellOrder
        //1. When you supply SellOrderRequest as null, it should throw ArgumentNullException.
        [Fact]
        public void CreateSellOrder_BuyOrderRequestIsNull()
        {
            // Arrange
            SellOrderRequest? sellOrderRequest = null;

            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                //Act
                _stocksService.CreateSellOrder(sellOrderRequest)
            );
        }

        //When you supply sellOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public void CreateSellOrder_QuantityIsZero()
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
            Assert.ThrowsAsync<ArgumentException>(() =>
                //Act
                _stocksService.CreateSellOrder(sellOrderRequest)
            );
        }

        // When you supply sellOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.
        [Fact]
        public void CreateSellOrder_QuantityIsOverMaximum()
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
            Assert.ThrowsAsync<ArgumentException>(() =>
                //Act
                _stocksService.CreateSellOrder(sellOrderRequest)
            );
        }

        //When you supply sellOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public void CreateSellOrder_PriceIsZero()
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
            Assert.ThrowsAsync<ArgumentException>(() =>
                //Act
                _stocksService.CreateSellOrder(sellOrderRequest)
            );
        }

        // When you supply sellOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.
        [Fact]
        public void CreateSellOrder_PriceIsOverMaximum()
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
            Assert.ThrowsAsync<ArgumentException>(() =>
                //Act
                _stocksService.CreateSellOrder(sellOrderRequest)
            );
        }

        //When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException
        [Fact]
        public void CreateSellOrder_StockSymbolIsNull()
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
            var exception = Assert.ThrowsAsync<ArgumentException>(() =>
                //Act
                _stocksService.CreateSellOrder(sellOrderRequest)
            );

            _outputHelper.WriteLine($"Exception message: {exception?.Result.Message}");
        }

        //When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification, it should be equal or newer date than 2000-01-01), it should throw ArgumentException.
        [Fact]
        public void CreateSellOrder_InvalidDate()
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
            var exception = Assert.ThrowsAsync<ArgumentException>(() =>
                //Act
                _stocksService.CreateSellOrder(sellOrderRequest)
            );

            _outputHelper.WriteLine($"Exception message: {exception?.Result.Message}");
        }

        // If you supply all valid values, it should be successful and return an object of SellOrderResponse type with auto-generated SellOrderID (guid).
        [Fact]
        public async void CreateSellOrder_ValidRequest()
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
            SellOrderResponse responseFromCreate = await _stocksService.CreateSellOrder(sellOrderRequest);

            //Assert
            Assert.NotEqual(responseFromCreate.SellOrderID, Guid.Empty);

            _outputHelper.WriteLine($"Sell order response object: {responseFromCreate.ToString()}");
        }

        #endregion

        #region GetAllBuyOrders
        //1. When you invoke this method, by default, the returned list should be empty.
        [Fact]
        public async void GetAllBuyOrders_EmptyList()
        {
            List<BuyOrderResponse> buyOrdersFromGet = await _stocksService.GetBuyOrders();

            Assert.Empty(buyOrdersFromGet);
        }

        //2. When you first add few buy orders using CreateBuyOrder() method; and then invoke GetAllBuyOrders() method; the returned list should contain all the same buy orders.

        [Fact]
        public async void GetAllBuyOrders_AddFewOrders()
        {
            //Arrange 
            List<BuyOrderResponse> buyOrders = new List<BuyOrderResponse>();

            List<BuyOrderRequest> buyOrderRequests = new List<BuyOrderRequest>()
            {
                new BuyOrderRequest()
                {
                    StockSymbol = "AAPL",
                    StockName = "Apple Inc.",
                    DateAndTimeOfOrder = DateTime.Now,
                    Quantity = 2,
                    Price = 12
                },
                new BuyOrderRequest()
                {
                    StockSymbol = "GOOGL",
                    StockName = "Alphabet Inc.",
                    DateAndTimeOfOrder = DateTime.Now,
                    Quantity = 3,
                    Price = 15
                }
            };

            foreach (var buyOrderRequest in buyOrderRequests)
            {
                BuyOrderResponse buyOrderResponse = await _stocksService.CreateBuyOrder(buyOrderRequest);

                buyOrders.Add(buyOrderResponse);
            }

            List<BuyOrderResponse> buyOrdersFromGet = await _stocksService.GetBuyOrders();

            foreach (var buyOrderFromGet in buyOrdersFromGet)
            {
                Assert.Contains(buyOrderFromGet, buyOrders);
            }


        }
        #endregion

        #region GetAllSellOrders
        //1. When you invoke this method, by default, the returned list should be empty.
        [Fact]
        public async void GetAllSellOrders_EmptyList()
        {
            List<SellOrderResponse> sellOrdersFromGet = await _stocksService.GetSellOrders();

            Assert.Empty(sellOrdersFromGet);
        }

        //2. When you first add few sell orders using CreateSellOrder() method; and then invoke GetAllSellOrders() method; the returned list should contain all the same sell orders.
        [Fact]
        public async void GetAllSellOrders_AddFewOrders()
        {
            //Arrange 
            List<SellOrderResponse> sellOrders = new List<SellOrderResponse>();

            List<SellOrderRequest> sellOrderRequests = new List<SellOrderRequest>()
            {
                new SellOrderRequest()
                {
                    StockSymbol = "AAPL",
                    StockName = "Apple Inc.",
                    DateAndTimeOfOrder = DateTime.Now,
                    Quantity = 2,
                    Price = 12
                },
                new SellOrderRequest()
                {
                    StockSymbol = "GOOGL",
                    StockName = "Alphabet Inc.",
                    DateAndTimeOfOrder = DateTime.Now,
                    Quantity = 3,
                    Price = 15
                }
            };

            foreach (var sellRequest in sellOrderRequests)
            {
                SellOrderResponse sellOrderResponse = await _stocksService.CreateSellOrder(sellRequest);

                sellOrders.Add(sellOrderResponse);
            }

            List<SellOrderResponse> sellOrdersFromGet = await _stocksService.GetSellOrders();

            foreach (var sellFromGet in sellOrdersFromGet)
            {
                Assert.Contains(sellFromGet, sellOrdersFromGet);
            }
        }
        #endregion
    }
}