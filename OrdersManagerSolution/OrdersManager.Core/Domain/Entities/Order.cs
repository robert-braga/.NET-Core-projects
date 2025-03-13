using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersManager.Core.Domain.Entities
{
    public class Order
    {
        [Key]
        public Guid OrderId { get; set; }

        public string? OrderNumber {  get; set; }

        [Required(ErrorMessage = "Customer name is required.")]
        [MaxLength(50, ErrorMessage = "Customer name must not exceed 50 characters.")]
        public string? CustomerName { get; set; }

        [Required(ErrorMessage = "Order date is required")]
        public DateTime OrderDate { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Total amount must be a positive number.")]
        [Column(TypeName = "decimal")]
        public decimal TotalAmount { get; set; }
    }
}
