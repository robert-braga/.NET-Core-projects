using Microsoft.AspNetCore.Mvc;
using OrdersManager.Core.Domain.Entities;
using OrdersManager.Core.DTO;
using OrdersManager.Core.ServiceContracts;

namespace OrdersManager.WebAPI.Controllers.v2
{
    /// <summary>
    /// 
    /// </summary>
    [ApiVersion("2.0")]
    public class OrdersController : CustomControllerBase
    {
        private readonly IOrdersService _ordersService;

        public OrdersController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        // GET: api/Orders
        /// <summary>
        /// Example: returns just the order numbers in v2. Whole orders are returned in v1
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Consumes("application/json")]
        public async Task<ActionResult<List<string?>>> GetOrders()
        {
            var orders = await _ordersService.GetAllOrders();
            return Ok(orders.Select(temp => temp.OrderNumber));
        }


        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(Guid id, UpdateOrderRequest order)
        {
            if (id != order.OrderId)
            {
                return BadRequest();
            }

            var updatedOrder = await _ordersService.UpdateOrder(order);

            return Ok(updatedOrder);
        }

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder([Bind(nameof(AddOrderRequest.OrderNumber), nameof(AddOrderRequest.OrderDate), nameof(AddOrderRequest.CustomerName), nameof(AddOrderRequest.TotalAmount))] AddOrderRequest order)
        {
            var addedOrder = await _ordersService.AddOrder(order);

            return CreatedAtAction(nameof(PostOrder), new { id = addedOrder.OrderId }, addedOrder);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            bool isDeleted = await _ordersService.DeleteOrder(id);

            if (!isDeleted)
                return NotFound();

            return NoContent();
        }


    }
}
