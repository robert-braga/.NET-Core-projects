using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class StocksService : IStocksService
    {
        private readonly ApplicationDbContext _stockMarketDbContext;

        public StocksService(ApplicationDbContext stockMarketDbContext)
        {
            _stockMarketDbContext = stockMarketDbContext;
        }

        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            //checks if the input is null
            if (buyOrderRequest == null)
            {
                throw new ArgumentNullException(nameof(buyOrderRequest));
            }

            // validates the input, if respects the constraints
            ValidationHelper.ModelValidation(buyOrderRequest);

            // creates a new buy order
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();

            buyOrder.BuyOrderID = Guid.NewGuid();

            // adds the buy order to the database
            _stockMarketDbContext.BuyOrders.Add(buyOrder);
            await _stockMarketDbContext.SaveChangesAsync();

            return buyOrder.ToBuyOrderResponse();
        }

        public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            if (sellOrderRequest == null)
                throw new ArgumentNullException(nameof(sellOrderRequest));

            ValidationHelper.ModelValidation(sellOrderRequest);

            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            sellOrder.SellOrderID = Guid.NewGuid();

            _stockMarketDbContext.SellOrders.Add(sellOrder);
            await _stockMarketDbContext.SaveChangesAsync();

            return sellOrder.ToSellOrderResponse();
        }

        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            var orders = await _stockMarketDbContext.BuyOrders.OrderByDescending(x => x.DateAndTimeOfOrder).ToListAsync();

            return orders.Select(x => x.ToBuyOrderResponse()).ToList();
        }

        public async Task<List<SellOrderResponse>> GetSellOrders()
        {
            var orders = await _stockMarketDbContext.SellOrders.OrderByDescending(x => x.DateAndTimeOfOrder).ToListAsync();

            return orders.Select(x => x.ToSellOrderResponse()).ToList();
        }
    }
}
