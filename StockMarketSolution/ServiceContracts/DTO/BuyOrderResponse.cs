using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class BuyOrderResponse : IOrderResponse
    {
        /// <summary>
        /// Unique identifier of the buy order
        /// </summary>
        public Guid BuyOrderID { get; set; }

        /// <summary>
        /// Stock symbol
        /// </summary>
        public string StockSymbol { get; set; }

        /// <summary>
        /// Name of the stock
        /// </summary>
        public string StockName { get; set; }

        /// <summary>
        /// Date and time of the order
        /// </summary>
        public DateTime DateAndTimeOfOrder { get; set; }

        /// <summary>
        /// Quantity of the stock
        /// </summary>
        public uint Quantity { get; set; }

        /// <summary>
        /// Price of the stock
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Total amount of the trade
        /// </summary>
        public double TradeAmount { get; set; }
        public EOrderType OrderType => EOrderType.BuyOrder;

        public override bool Equals(object? obj)
        {
            return obj is BuyOrderResponse response &&
                   BuyOrderID.Equals(response.BuyOrderID) &&
                   StockSymbol == response.StockSymbol &&
                   StockName == response.StockName &&
                   DateAndTimeOfOrder == response.DateAndTimeOfOrder &&
                   Quantity == response.Quantity &&
                   Price == response.Price &&
                   TradeAmount == response.TradeAmount;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string? ToString()
        {
            return $"Buy order ID: {BuyOrderID}, Stock symbol: {StockSymbol}, Stock name: {StockName}, Date and time of order: {DateAndTimeOfOrder}, Quantity: {Quantity}, Price: {Price}, Trade amount: {TradeAmount}";
        }


    }

    public static class BuyOrderResponseExtensions
    {
        /// <summary>
        /// Converts a BuyOrder to a BuyOrderResponse
        /// </summary>
        /// <param name="buyOrder">The BuyOrder object to convert</param>
        /// <returns>Returns converted BuyOrderResponse</returns>
        public static BuyOrderResponse ToBuyOrderResponse(this BuyOrder buyOrder)
        {
            return new BuyOrderResponse()
            {
                BuyOrderID = buyOrder.BuyOrderID,
                StockSymbol = buyOrder.StockSymbol,
                StockName = buyOrder.StockName,
                DateAndTimeOfOrder = buyOrder.DateAndTimeOfOrder,
                Quantity = buyOrder.Quantity,
                Price = buyOrder.Price,
                TradeAmount = buyOrder.Quantity * buyOrder.Price
            };
        }
    }
}
