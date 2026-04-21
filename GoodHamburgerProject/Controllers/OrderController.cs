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
    }
}
