using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using ServiceContracts;
using StockMarketSolution;
using StockMarketSolution.Controllers;
using StockMarketSolution.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Tests.ControllerTests
{
    public class StocksControllerTests
    {
        private readonly Mock<IFinnhubService> _finnhubServiceMock;
        private readonly IFinnhubService _finnhubService;

        private readonly Mock<IOptions<TradingOptions>> _tradingOptionsMock;
        private readonly IOptions<TradingOptions> _tradingOptions;

        public StocksControllerTests()
        {
            _finnhubServiceMock = new Mock<IFinnhubService>();
            _tradingOptionsMock = new Mock<IOptions<TradingOptions>>();

            _tradingOptionsMock
                .Setup(x => x.Value)
                .Returns(new TradingOptions()
                {
                    Top25PopularStocks = "AAPL,MSFT,GOOGL"
                });

            _finnhubService = _finnhubServiceMock.Object;
            _tradingOptions = _tradingOptionsMock.Object;
        }

        [Fact]
        public async Task Explore_NullStockSymbol_ShouldReturnExploreViewWithListOfStocks()
        {
            StocksController stocksController = new StocksController(_finnhubService, _tradingOptions);

            List<Dictionary<string, string>> stocks = new List<Dictionary<string, string>>()
            {
                new Dictionary<string, string>()
                {
                    { "symbol", "AAPL" },
                    { "description", "Apple Inc." }
                },
                new Dictionary<string, string>()
                {
                    { "symbol", "MSFT" },
                    { "description", "Microsoft Corporation" }
                },
                new Dictionary< string, string> ()
                {
                    { "symbol", "GOOGL" },
                    { "description", "Alphabet Inc." }
                }
            };

            List<Stock> stocksList = stocks.Select(x => new Stock()
            {
                StockSymbol = x["symbol"],
                StockName = x["description"]
            }).ToList();

            _finnhubServiceMock
                .Setup(x => x.GetStocks())
                .Returns(Task.FromResult(stocks));

            IActionResult actionResult = await stocksController.Explore(null as string);

            ViewResult viewResult = Assert.IsType<ViewResult>(actionResult);

            viewResult.ViewData.Model.Should().BeAssignableTo<IEnumerable<Stock>>();
            viewResult.ViewData.Model.Should().BeEquivalentTo(stocksList);
        }
    }
}
