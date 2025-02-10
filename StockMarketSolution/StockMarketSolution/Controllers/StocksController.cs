using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts;
using StockMarketSolution.Models;

namespace StockMarketSolution.Controllers
{
    [Route("[controller]")]
    public class StocksController : Controller
    {
        private readonly IFinnhubService _finnhubService;
        private readonly TradingOptions _tradingOptions;

        public StocksController(IFinnhubService finnhubService, IOptions<TradingOptions> tradingOptions)
        {
            _finnhubService = finnhubService;
            _tradingOptions = tradingOptions.Value;
        }

        [Route("/")]
        [Route("[action]/{stock?}")]
        public async Task<IActionResult> Explore(string? stock)
        {
            List<Stock> stocks = new List<Stock>();

            var popularStocks = _tradingOptions?.Top25PopularStocks?.Split(',').ToList();

            var stocksList = await _finnhubService.GetStocks();

            if(popularStocks != null && stocksList != null)
            {
                stocksList = stocksList
                    .Where(x => popularStocks.Contains(Convert.ToString(x["symbol"])))
                    .ToList();

                stocks = stocksList
                    .Select(x => new Stock()
                        {
                            StockSymbol = Convert.ToString(x["symbol"]),
                            StockName = Convert.ToString(x["description"])
                        })
                    .ToList();
            }

            ViewBag.Stock = stock;

            return View(stocks);
        }
    }
}
