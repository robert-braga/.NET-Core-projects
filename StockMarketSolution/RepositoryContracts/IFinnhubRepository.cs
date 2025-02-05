using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryContracts
{
    /// <summary>
    /// Repository interface for accessing data from the Finnhub API.
    /// </summary>
    public interface IFinnhubRepository
    {
        /// <summary>
        /// Get the company profile for a stock symbol.
        /// </summary>
        /// <param name="stockSymbol">Stock symbol used to search the company profile</param>
        /// <returns>Data about the company</returns>
        Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol);

        /// <summary>
        /// Get the stock price quote for a stock symbol.
        /// </summary>
        /// <param name="stockSymbol">stock symbol to search</param>
        /// <returns>Stock price quote for stock symbol</returns>
        Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol);

        /// <summary>
        /// Get the stocks available in US.
        /// </summary>
        /// <returns></returns>
        Task<List<Dictionary<string, string>>?> GetStocks();

        /// <summary>
        /// Search for stocks based on the stock symbol.
        /// </summary>
        /// <param name="stockSymbolToSearch">Stock symbol to search</param>
        /// <returns>Found stock based on given stock symbol</returns>
        Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch);
    }
}
