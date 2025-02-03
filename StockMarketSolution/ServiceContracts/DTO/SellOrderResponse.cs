using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class SellOrderResponse : IOrderResponse
    {
        /// <summary>
         /// Unique identifier of the sell order
         /// </summary>
        public Guid SellOrderID { get; set; }

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
        public int Quantity { get; set; }

        /// <summary>
        /// Price of the stock
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Total amount of the trade
        /// </summary>
        public double TradeAmount { get; set; }

        public EOrderType OrderType => EOrderType.SellOrder;

        public override bool Equals(object? obj)
        {
            return obj is SellOrderResponse response &&
                   SellOrderID.Equals(response.SellOrderID) &&
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
            return $"Sell order ID: {SellOrderID}, Stock symbol: {StockSymbol}, Stock name: {StockName}, Date and time of order: {DateAndTimeOfOrder}, Quantity: {Quantity}, Price: {Price}, Trade amount: {TradeAmount}";
        }
    }

    public static class SellOrderResponseExtensions
    {
        public static SellOrderResponse ToSellOrderResponse(this SellOrder sellOrder)
        {
            return new SellOrderResponse
            {
                SellOrderID = sellOrder.SellOrderID,
                StockSymbol = sellOrder.StockSymbol,
                StockName = sellOrder.StockName,
                DateAndTimeOfOrder = sellOrder.DateAndTimeOfOrder,
                Quantity = sellOrder.Quantity,
                Price = sellOrder.Price,
                TradeAmount = sellOrder.Price * sellOrder.Quantity
            };
        }
    }
}
