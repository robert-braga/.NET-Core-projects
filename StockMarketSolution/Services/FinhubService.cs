using ServiceContracts;
using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RepositoryContracts; // Add this using directive


namespace Services
{
    public class FinhubService : IFinnhubService
    {
        private readonly IFinnhubRepository _finnhubRepository;

        public FinhubService(IFinnhubRepository finnhubRepository)
        {
            _finnhubRepository = finnhubRepository;
        }

        public async Task<Dictionary<string,object>>? GetCompanyProfile(string stockSymbol)
        {
            var responseDictionary = await _finnhubRepository.GetCompanyProfile(stockSymbol);

            return responseDictionary;
        }

        public async Task<Dictionary<string, object>>? GetStockPriceQuote(string stockSymbol)
        {
            var responseDictionary = await _finnhubRepository.GetStockPriceQuote(stockSymbol);  

            return responseDictionary;
        }

        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            var response = await _finnhubRepository.GetStocks();

            return response;
        }

        public async Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
        {
            var responseDictionary = await _finnhubRepository.SearchStocks(stockSymbolToSearch);

            return responseDictionary;
        }
    }
}
