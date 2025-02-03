using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class BuyOrderRequest : IValidatableObject
    {
        /// <summary>
        /// Stock symbol
        /// </summary>
        [Required(ErrorMessage = "Stock symbol cannot be null or empty")]
        public string StockSymbol { get; set; }

        /// <summary>
        /// Name of the stock
        /// </summary>
        [Required(ErrorMessage = "Stock name cannot be null or empty")]
        public string StockName { get; set; }

        /// <summary>
        /// Date and time of the order
        /// </summary>
        public DateTime DateAndTimeOfOrder { get; set; }

        /// <summary>
        /// Quantity of the stock
        /// </summary>
        [Range(1, 100000, ErrorMessage = "You can buy maximum 100000 shares in  a single order. Minimum is 1")]
        public int Quantity { get; set; }

        /// <summary>
        /// Price of the stock
        /// </summary>
        [Range(1, 10000, ErrorMessage = "The maximum price of stock is 1000. Minimum is 1")]
        public double Price { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            if (DateAndTimeOfOrder < DateTime.Parse("01/01/2000"))
            {
                results.Add(new ValidationResult("Date and time of order cannot be less than 01/01/2000"));
            }

            return results;
        }

        public BuyOrder ToBuyOrder()
        {
            return new BuyOrder()
            {
                StockSymbol = StockSymbol,
                StockName = StockName,
                DateAndTimeOfOrder = DateAndTimeOfOrder,
                Quantity = Quantity,
                Price = Price
            };
        }
    }
}
