using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersManager.Core.Domain.Entities
{
    public class OrderItem
    {
        [Key]
        public Guid OrderItemId { get; set; }

        [Required(ErrorMessage = "{0} must be provided")]
        public Guid OrderId { get; set; }

        [Required(ErrorMessage = "{0} is mandatory.")]
        [MaxLength(50, ErrorMessage = "{0} must not exceed {1} characters.")]
        public string? ProductName { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "{0} must be a positive number.")]
        public int Quantity { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "{0} must be a positive number.")]
        [Column(TypeName = "decimal")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal")]
        public decimal TotalPrice { get; set; }

    }
}
