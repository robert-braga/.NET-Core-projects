using Microsoft.AspNetCore.Mvc;
using OrdersManager.Core.Domain.Entities;
using OrdersManager.Core.DTO;
using OrdersManager.Core.ServiceContracts;

namespace OrdersManager.WebAPI.Controllers.v1
{
    /// <summary>
    /// 
    /// </summary>
    [ApiVersion("1.0")]
    public class OrdersController : CustomControllerBase
    {
        private readonly IOrdersService _ordersService;

        public OrdersController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        // GET: api/Orders
        [HttpGet]
        [Consumes("application/json")] //eother here or in Program.cs
        public async Task<ActionResult<List<OrderResponse>>> GetOrders()
        {
            var orders = await _ordersService.GetAllOrders();
            return Ok(orders);
        }

        // GET: api/Orders/5
        /// <summary>
        /// To get an order by its id
        /// </summary>
        /// <param name="id">Order ID to search</param>
        /// <returns>The  order found</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResponse>> GetOrder(Guid id)
        {
            var order = await _ordersService.GetOrderByOrderID(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
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
