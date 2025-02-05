using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class FinnhubRepository : IFinnhubRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public FinnhubRepository(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
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

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
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

            var responseDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);

            if (responseDictionary == null)
            {
                throw new InvalidOperationException($"No response from the server.");
            }

            if (responseDictionary.ContainsKey("error"))
            {
                throw new InvalidOperationException($"Error: {responseDictionary["error"]}");
            }

            return responseDictionary;
        }

        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            HttpClient client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Finnhub-Token", _configuration["finhubToken"]);

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://finnhub.io/api/v1/quote??exchange=US")
            };

            var response = await client.SendAsync(httpRequestMessage);

            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var responseList = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(content);

                if (responseList == null)
                {
                    throw new InvalidOperationException($"No response from the server.");
                }

                return responseList;
            }
            else
            {
                throw new InvalidOperationException($"Error: {response.StatusCode} | Error message: {content}");
            }
        }

        public async Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
        {
            HttpClient client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Add("X-Finnhub-Token", _configuration["finhubToken"]);

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://finnhub.io/api/v1/search?q={stockSymbolToSearch}&exchange=US")
            };

            var response = await client.SendAsync(httpRequestMessage);

            var content = await response.Content.ReadAsStringAsync();

            var responseDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);

            if (responseDictionary == null)
            {
                throw new InvalidOperationException($"No response from the server.");
            }

            if (responseDictionary.ContainsKey("error"))
            {
                throw new InvalidOperationException($"Error: {responseDictionary["error"]}");
            }

            return responseDictionary;
        }
    }
}
