namespace StockMarketSolution.Models
{
    /// <summary>
    /// View model, used to send model object from controller to Stocks/Explore view
    /// </summary>
    public class Stock
    {
        public string? StockSymbol { get; set; }
        public string? StockName { get; set; }
    }
}
