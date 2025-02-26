using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fizzler;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace Tests.IntegrationTests
{
    public class TradeControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _httpClient;

        public TradeControllerIntegrationTest(CustomWebApplicationFactory factory)
        {
            _httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task Index_WhenRouteToAStock_ShouldReturnAView()
        {
            // Arrange
            var stockSymbol = "MSFT";

            // Act
            HttpResponseMessage response = await _httpClient.GetAsync($"Trade/Index/{stockSymbol}");


            var responseString = await response.Content.ReadAsStringAsync();
            
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(responseString);

            HtmlNode document = htmlDocument.DocumentNode;

            document.QuerySelectorAll(".price").Should().NotBeNull();
        }
    }
}
