using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
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

        public TradeController(IOptions<TradingOptions> tradingOptions, IConfiguration configuration, IFinnhubService finhubService, IStocksService stocksService)
        {
            this._tradingOptions = tradingOptions.Value;
            _configuration = configuration;
            _finnhubService = finhubService;
            _stocksService = stocksService;
        }

        [Route("[action]")]
        [Route("/")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (string.IsNullOrWhiteSpace(_tradingOptions.DefaultStockSymbol))
                _tradingOptions.DefaultStockSymbol = "MSFT";

            StockTrade stockTrade = new StockTrade()
            {
                StockSymbol = _tradingOptions.DefaultStockSymbol
            };

            var companyProfileResponse = await _finnhubService.GetCompanyProfile(_tradingOptions.DefaultStockSymbol);

            var quoteResponse = await _finnhubService.GetStockPriceQuote(_tradingOptions.DefaultStockSymbol);

            if( companyProfileResponse != null && quoteResponse != null)
            {
                stockTrade.StockName = Convert.ToString(companyProfileResponse["name"]);
                stockTrade.Price = Convert.ToDouble(quoteResponse["c"]);
            }

            ViewBag.finhubToken = _configuration["finhubToken"];

            return View(stockTrade);
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Orders()
        {
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
        public async Task<IActionResult> BuyOrder(BuyOrder buyOrder)
        {
            if(!ModelState.IsValid)
            {
                StockTrade stockTrade = new StockTrade()
                {
                    StockSymbol = buyOrder.StockSymbol,
                    StockName = buyOrder.StockName,
                    Price = buyOrder.Price,
                    Quantity = buyOrder.Quantity
                };

                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                return View("Index", stockTrade);
            }

            BuyOrderRequest buyOrderRequest = new BuyOrderRequest()
            {
                StockName = buyOrder.StockName,
                StockSymbol = buyOrder.StockSymbol,
                DateAndTimeOfOrder = DateTime.Now,
                Price = buyOrder.Price,
                Quantity = buyOrder.Quantity
            };

            BuyOrderResponse buyOrderResponse = await _stocksService.CreateBuyOrder(buyOrderRequest);
            return RedirectToAction("Orders", "Trade");
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> SellOrder(SellOrder sellOrder)
        {
            if (!ModelState.IsValid)
            {
                StockTrade stockTrade = new StockTrade()
                {
                    StockSymbol = sellOrder.StockSymbol,
                    StockName = sellOrder.StockName,
                    Price = sellOrder.Price,
                    Quantity = sellOrder.Quantity
                };

                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                return View("Index", stockTrade);
            }

            SellOrderRequest sellOrderRequest = new SellOrderRequest()
            {
                StockName = sellOrder.StockName,
                StockSymbol = sellOrder.StockSymbol,
                DateAndTimeOfOrder = DateTime.Now,
                Price = sellOrder.Price,
                Quantity = sellOrder.Quantity
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
