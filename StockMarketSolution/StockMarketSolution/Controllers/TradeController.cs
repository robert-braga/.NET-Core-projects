using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using StockMarketSolution.Filters.ActionFilters;
using StockMarketSolution.Models;

namespace StockMarketSolution.Controllers
{
    [Route("[controller]")]
    public class TradeController : Controller
    {
        private readonly TradingOptions _tradingOptions;
        private readonly IConfiguration _configuration;
        private readonly IFinnhubService _finnhubService;
        private readonly IStocksService _stocksService;
        private readonly ILogger<TradeController> _logger;  

        public TradeController(IOptions<TradingOptions> tradingOptions, IConfiguration configuration, IFinnhubService finhubService, IStocksService stocksService, ILogger<TradeController> logger)
        {
            this._tradingOptions = tradingOptions.Value;
            _configuration = configuration;
            _finnhubService = finhubService;
            _stocksService = stocksService;
            _logger = logger;
        }

        [Route("[action]/{stockSymbol?}")]
        [HttpGet]
        public async Task<IActionResult> Index(string? stockSymbol)
        {
            if (string.IsNullOrWhiteSpace(_tradingOptions.DefaultStockSymbol))
                _tradingOptions.DefaultStockSymbol = "MSFT";

            if (_tradingOptions.DefaultOrderQuantity == null)
                _tradingOptions.DefaultOrderQuantity = 100;

            StockTrade stockTrade = new StockTrade()
            {
                StockSymbol = stockSymbol ?? _tradingOptions.DefaultStockSymbol
            };

            var companyProfileResponse = await _finnhubService.GetCompanyProfile(stockTrade.StockSymbol);

            var quoteResponse = await _finnhubService.GetStockPriceQuote(stockTrade.StockSymbol);

            if (companyProfileResponse != null && quoteResponse != null)
            {
                stockTrade.StockName = Convert.ToString(companyProfileResponse["name"]);
                stockTrade.Price = Convert.ToDouble(quoteResponse["c"]);
                stockTrade.Quantity = _tradingOptions.DefaultOrderQuantity.Value;

            }

            ViewBag.finhubToken = _configuration["finhubToken"];

            return View(stockTrade);
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            _logger.LogInformation("Getting orders");

            List<BuyOrderResponse> buyOrders = await _stocksService.GetBuyOrders();

            List<SellOrderResponse> sellOrders = await _stocksService.GetSellOrders();

            Orders orders = new Orders()
            {
                BuyOrders = buyOrders,
                SellOrders = sellOrders
            };

            return View(orders);
        }

        [Route("[action]")]
        [HttpPost]
        [TypeFilter(typeof(RedirectActionFilter))]
        public async Task<IActionResult> BuyOrder(BuyOrder order)
        {
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest()
            {
                StockName = order.StockName,
                StockSymbol = order.StockSymbol,
                DateAndTimeOfOrder = DateTime.Now,
                Price = order.Price,
                Quantity = order.Quantity
            };

            BuyOrderResponse buyOrderResponse = await _stocksService.CreateBuyOrder(buyOrderRequest);
            return RedirectToAction("Orders", "Trade");
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> SellOrder(SellOrder order)
        {
            SellOrderRequest sellOrderRequest = new SellOrderRequest()
            {
                StockName = order.StockName,
                StockSymbol = order.StockSymbol,
                DateAndTimeOfOrder = DateTime.Now,
                Price = order.Price,
                Quantity = order.Quantity
            };

            SellOrderResponse sellOrderResponse = await _stocksService.CreateSellOrder(sellOrderRequest);
            return RedirectToAction("Orders", "Trade");
        }

        [Route("[action]")]
        public async Task<IActionResult> OrdersPDF()
        {
            List<BuyOrderResponse> buyOrders = await _stocksService.GetBuyOrders();

            List<SellOrderResponse> sellOrders = await _stocksService.GetSellOrders();

            List<IOrderResponse> orders = new List<IOrderResponse>();
            orders.AddRange(buyOrders);
            orders.AddRange(sellOrders);

            orders = orders.OrderByDescending(x => x.DateAndTimeOfOrder).ToList();

            return new ViewAsPdf("OrdersPDF", orders, ViewData) { 
                 PageMargins = new Rotativa.AspNetCore.Options.Margins(20, 20, 20, 20),
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }
    }
}
