using GoodHamburgerProject.DTOs;
using GoodHamburgerProject.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburgerProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(IOrderService orderService) : ControllerBase
    {
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

        [HttpGet]
        public async Task<ActionResult<List<OrderResponseDTO>>> GetAllOrders()
        {
            return Ok(await orderService.GetAllOrdersAsync());

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<OrderResponseDTO>>> GetOrderById(Guid id)
        {
            var order = await orderService.GetOrderByIdAsync(id);

            if (order is null)
            {
                return NotFound("Order not found");
            }


            return Ok(order);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(Guid id)
        {
            var deleted = await orderService.DeleteOrderAsync(id);
            return deleted ? NoContent() : NotFound("No Orders ware found with that ID.");
        }
    }
}
