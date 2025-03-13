using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrdersManager.Core.Domain.Entities;
using OrdersManager.Core.DTO;
using OrdersManager.Core.ServiceContracts;
using OrdersManager.Infrastructure.DatabaseContext;

namespace OrdersManager.WebAPI.Controllers
{
    public class OrderItemsController : CustomControllerBase
    {
        private readonly IOrderItemsService _orderItemsService;

        public OrderItemsController(IOrderItemsService orderItemsService)
        {
            _orderItemsService = orderItemsService;
        }

        // GET: api/OrderItems
        [HttpGet("{order}/items")]
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetOrderItems(Guid order)
        {
            var orderItems = await _orderItemsService.GetAllOrderItems(order);

            return Ok(orderItems);
        }

        // GET: api/OrderItems/5
        [HttpGet("{order}/items/{id}")]
        public async Task<ActionResult<OrderItem>> GetOrderItem(Guid order, Guid id)
        {
            var orderItem = await _orderItemsService.GetOrderItemByID(id);

            if (orderItem == null)
            {
                return NotFound();
            }

            return Ok(orderItem);
        }

        // PUT: api/OrderItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{order}/items/{id}")]
        public async Task<IActionResult> PutOrderItem(Guid order, Guid id, UpdateOrderItemRequest orderItem)
        {
            if (id != orderItem.OrderItemId)
            {
                return BadRequest();
            }

            await _orderItemsService.UpdateOrderItem(orderItem);

            return NoContent();
        }

        // POST: api/OrderItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{order}/items")]
        public async Task<ActionResult<OrderItem>> PostOrderItem(Guid order, AddOrderItemRequest orderItem)
        {
            var addedOrderItem = await _orderItemsService.AddOrderItem(orderItem);   

            return CreatedAtAction(nameof(PostOrderItem), new { id = addedOrderItem.OrderItemId }, addedOrderItem);
        }

        // DELETE: api/OrderItems/5
        [HttpDelete("{order}/items/{id}")]
        public async Task<IActionResult> DeleteOrderItem(Guid order, Guid id)
        {
            var isDeleted = await _orderItemsService.DeleteOrderItem(id);

            if (!isDeleted)
                return NotFound();

            return NoContent();
        }
    }
}
