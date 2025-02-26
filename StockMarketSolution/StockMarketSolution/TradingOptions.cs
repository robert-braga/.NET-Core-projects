namespace StockMarketSolution
{
    /// <summary>
    /// Options pattern for "StockPrice" configuration
    /// </summary>
    public class TradingOptions
    {
        public string? DefaultStockSymbol { get; set; }
        public int? DefaultOrderQuantity { get; set; }
        public string? Top25PopularStocks { get; set; }
    }
}
