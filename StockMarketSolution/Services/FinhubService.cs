using ServiceContracts;
using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json; // Add this using directive


namespace Services
{
    public class FinhubService : IFinnhubService
    {
        private readonly IHttpClientFactory _httpClientFactory; // Complete the declaration
        private readonly IConfiguration _configuration;

        public FinhubService(IHttpClientFactory httpClientFactory, IConfiguration configuration) // Add constructor to initialize IHttpClientFactory
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<Dictionary<string,object>>? GetCompanyProfile(string stockSymbol)
        {
            HttpClient client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Add("X-Finnhub-Token", _configuration["finhubToken"]);

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}")
            };

            var response = await client.SendAsync(httpRequestMessage);

            var content = await response.Content.ReadAsStringAsync();

            var responsDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);

            if (responsDictionary == null)
            {
                throw new InvalidOperationException($"No response from the server.");
            }

            if (responsDictionary.ContainsKey("error"))
            {
                throw new InvalidOperationException($"Error: {responsDictionary["error"]}");
            }

            return responsDictionary;
        }

        public async Task<Dictionary<string, object>>? GetStockPriceQuote(string stockSymbol)
        {
            HttpClient client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Add("X-Finnhub-Token", _configuration["finhubToken"]);

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}")
            };

            var response = await client.SendAsync(httpRequestMessage);

            var content = await response.Content.ReadAsStringAsync();

            var responsDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);

            if (responsDictionary == null)
            {
                throw new InvalidOperationException($"No response from the server.");
            }

            if (responsDictionary.ContainsKey("error"))
            {
                throw new InvalidOperationException($"Error: {responsDictionary["error"]}");
            }

            return responsDictionary;
        }
    }
}
