using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersManager.Core.DTO
{
    public class AddOrderRequest
    {
        public string? OrderNumber {  get; set; }
        public string? CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount  { get; set; }

    }
}
