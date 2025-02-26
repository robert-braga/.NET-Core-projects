using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts;

namespace StockMarketSolution.ViewComponents
{
    public class SelectedStockViewComponent : ViewComponent
    {
        private readonly IStocksService _stocksService;
        private readonly TradingOptions tradingOptions;
        private readonly IFinnhubService _finnhubService;
        private readonly IConfiguration _configuration;

        public SelectedStockViewComponent(IStocksService stocksService, IOptions<TradingOptions> tradingOptions, IFinnhubService finnhubService, IConfiguration configuration)
        {
            _stocksService = stocksService;
            this.tradingOptions = tradingOptions.Value;
            _finnhubService = finnhubService;
            _configuration = configuration;
        }

        public async Task<IViewComponentResult> InvokeAsync(string? stock)
        {
            Dictionary<string, object>? companyProfileDict = null;

            if (stock != null)
            {
                companyProfileDict = await _finnhubService.GetCompanyProfile(stock);
                var stockPriceDict = await _finnhubService.GetStockPriceQuote(stock);
                if (stockPriceDict != null && companyProfileDict != null)
                {
                    companyProfileDict.Add("price", stockPriceDict["c"]);
                }
            }

            if (companyProfileDict != null && companyProfileDict.ContainsKey("logo"))
                return View(companyProfileDict);
            else
                return Content("");
        }
    }
}
