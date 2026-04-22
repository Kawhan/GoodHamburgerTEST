using GoodHamburgerProject.DTOs;
using GoodHamburgerProject.Exceptions;
using GoodHamburgerProject.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburgerProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(IOrderService orderService) : ControllerBase
    {

        /// <summary>
        /// Creates a new order with the provided items, calculates total amount,
        /// and applies the best available discount.
        /// </summary>
        /// <param name="request">Order data including the list of items</param>
        /// <returns>The created order with calculated totals and discounts</returns>
        /// <response code="201">Order successfully created</response>
        /// <response code="400">
        /// Invalid request data, duplicate items, or invalid item type
        /// </response>
        /// <response code="404">
        /// Burger or accompaniment not found
        /// </response>
        /// <response code="409">
        /// Business rule violations (e.g., more than one burger in the order)
        /// </response>
        [ProducesResponseType(typeof(OrderResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPost]
        public async Task<ActionResult<OrderResponseDTO>> CreateOrder(CreateOrderRequestDTO request)
        {
            var result = await orderService.CreateOrderAsync(request);

            return CreatedAtAction(
                nameof(CreateOrder),
                new { id = result.Id },
                result
            );
        }

        /// <summary>
        /// Retrieves all orders with their items, totals, and applied discounts.
        /// </summary>
        /// <returns>A list of orders</returns>
        /// <response code="200">Orders successfully retrieved</response>
        /// <response code="204">No orders found</response>
        [ProducesResponseType(typeof(List<OrderResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpGet]
        public async Task<ActionResult<List<OrderResponseDTO>>> GetAllOrders()
        {
            return Ok(await orderService.GetAllOrdersAsync());
        }

        /// <summary>
        /// Retrieves an order by its unique identifier, including items,
        /// totals, and applied discounts.
        /// </summary>
        /// <param name="id">Order unique identifier (GUID)</param>
        /// <returns>The requested order</returns>
        /// <response code="200">Order successfully retrieved</response>
        /// <response code="404">Order not found</response>
        [ProducesResponseType(typeof(OrderResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<List<OrderResponseDTO>>> GetOrderById(Guid id)
        {
            var order = await orderService.GetOrderByIdAsync(id);
            return Ok(order);
        }


        /// <summary>
        /// Deletes an order by its unique identifier.
        /// </summary>
        /// <param name="id">Order unique identifier (GUID)</param>
        /// <returns>No content if the deletion is successful</returns>
        /// <response code="204">Order successfully deleted</response>
        /// <response code="404">Order not found</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(Guid id)
        {
            var deleted = await orderService.DeleteOrderAsync(id);
            return deleted ? NoContent() : NotFound("No Orders ware found with that ID.");
        }

        /// <summary>
        /// Updates an existing order by its unique identifier, including items,
        /// recalculating totals and reapplying discounts.
        /// </summary>
        /// <param name="id">Order unique identifier (GUID)</param>
        /// <param name="request">Updated order data including items</param>
        /// <returns>No content if the update is successful</returns>
        /// <response code="204">Order successfully updated</response>
        /// <response code="400">
        /// Invalid request data or invalid item type
        /// </response>
        /// <response code="404">Order not found</response>
        /// <response code="409">
        /// Business rule violations (e.g., more than one burger or duplicate items)
        /// </response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPut("{id}")]
        public async Task<ActionResult<OrderResponseDTO>> UpdateOrder(Guid id, UpdateOrderRequestDTO request)
        {
            var updated = await orderService.UpdateOrderAsync(id, request);
            return updated ? NoContent() : NotFound("No Orders were found with that ID.");
        }
    }
}
